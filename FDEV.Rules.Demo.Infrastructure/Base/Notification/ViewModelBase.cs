using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/***********************************************************************************************************
 **  The absolutely most annoying thing about MVVM is implementing INotifyPropertyChanged, and seeing the  **
 **  verbose property setters makes me want to do bad things to myself :-)  I have therefore searched for  **
 **  the best ways to circumvent this, and have found what I consider the most elegant, clean, readable -  **
 **  and ridiculously easy to maintain pattern.             **
 ************************************************************************************************************/

namespace FSites.Core.Domain.Dynamic
{
    /// <summary>
    /// The code is self-explanatory and/or documented in the class! The key ingredients is the use of an attribute
    /// 'TriggerOnChangeAttribute' and inversion of dependency - so that the properties, methods and commands that
    /// are dependent on a given property-change, will subscribe to that property. That means that rather than the
    /// property having to know who to notify when it changes, the dependency is declared on the listener side of things. Both Properties, Methods and Commands can listen for changes.
    /// </summary>
    public class ViewModelBase : DynamicObject, INotifyPropertyChanged, IDataErrorInfo
    {
        /// <inheritdoc />
        /// <summary>
        /// Inherit class to gain access the cleanest implementation of INotifyPropertyChanged. Write smaller, cleaner
        /// and more maintainable view models, by inheriting this class and implement some other dynamic stuff! :-)
        /// Don not let the the Dynamic and Generic stuff put you of from using this base class, it really works and
        /// if you are missing any functionality, let me know ;-)  Kind regards, csharp@feltman.net
        /// </summary>
        protected ViewModelBase()
        {
            _propertyMap = MapDependencies<TriggerOnChangeAttribute>(() => GetType().GetProperties());
            _methodMap = MapDependencies<TriggerOnChangeAttribute>(() => GetType().GetMethods().Cast<MemberInfo>().Where(method => !method.Name.StartsWith(CanExecutePrefix)));
            _commandMap = MapDependencies<TriggerOnChangeAttribute>(() => GetType().GetMethods().Cast<MemberInfo>().Where(method => method.Name.StartsWith(CanExecutePrefix)));
            CreateCommands();
            VerifyTriggerProperties();
        }

        /// <summary>
        /// Dictionary that holds the dynamic data inserted with the setter methods below, and retrieved with any of the
        /// insanely many 'Getter' overloads - I wanted to ensure that you could get to the dynamic data in any way you
        /// would like or are used too. Let members stay public or protected, and discover all the possibilities :-)
        /// </summary>
        private readonly Dictionary<string, object> _values = new();

        #region Property Setters and dependencies

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var result = base.TrySetMember(binder, value);
            if (result) return true;

            Set(binder.Name, value);
            return true;
        }

        /// <summary>
        /// Set property value. If the property doesn't exist, it is created and value set.
        /// The main part of the method is replaced by a Dictionary extension
        /// </summary>
        public void Set<T>(string name, T value)
        {
            _values.Set(name, value);
            RaisePropertyChanged(name);
        }

        protected void Set<T>(Expression<Func<T>> expression, T value) => Set(PropertyName(expression), value);

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged.Raise(this, name);
            TriggerDependentProperties(name);
            TriggerDependentMethods(name);
            FireChangesOnDependentCommands(name);
        }
                
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDictionary<string, List<string>> _propertyMap;

        private void TriggerDependentProperties(string name)
        {
            if (!_propertyMap.ContainsKey(name)) return;

            _propertyMap[name].Each(RaisePropertyChanged);
        }

        #endregion Property Setters and dependencies

        #region Property getters

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Get<object>(binder.Name);
            return result != null || base.TryGetMember(binder, out result);
        }

        protected T Get<T>(string name) => Get(name, default(T));
        protected T Get<T>(string name, T defaultValue) => _values.ContainsKey(name) ? (T) _values[name] : defaultValue;

        protected T Get<T>(string name, Func<T> initialValue)
        {
            if (_values.ContainsKey(name)) return (T) _values[name];

            Set(name, initialValue());
            return Get<T>(name);
        }

        protected T Get<T>(Expression<Func<T>> expression) => Get<T>(PropertyName(expression));

        protected T Get<T>(Expression<Func<T>> expression, T defaultValue) => Get(PropertyName(expression), defaultValue);

        protected T Get<T>(Expression<Func<T>> expression, Func<T> initialValue) => Get(PropertyName(expression), initialValue);

        private static string PropertyName<T>(Expression<Func<T>> expression) => expression.Body is not MemberExpression memberExpression
                ? throw new ArgumentException("expression must be a property expression")
                : memberExpression.Member.Name;

        private static IDictionary<string, List<string>> MapDependencies<T>(Func<IEnumerable<MemberInfo>> getInfo)
            where T : TriggerOnChangeAttribute
        {
            var dependencyMap = Enumerable.ToDictionary(getInfo(), p => p.Name, p => p.GetCustomAttributes(typeof(T), true).Cast<T>().Select(a => a.TriggerPropertyName).ToList());
            return InvertDependencies(dependencyMap);
        }

        private static IDictionary<string, List<string>> InvertDependencies(IDictionary<string, List<string>> map)
        {
            var values = map.Keys.SelectMany(key => map[key], (key, value) => new {Key = key, Value = value}).ToList();
            var uniqueValues = values.Select(x => x.Value).Distinct();
            return uniqueValues.ToDictionary(x => x, x => (values.Where(item => item.Value == x).Select(item => item.Key)).ToList());
        }

        private void VerifyTriggerProperties()
        {
            var methods = GetType().GetMethods().Cast<MemberInfo>();
            var properties = GetType().GetProperties();
            var propertyNames = methods.Union(properties)
                .SelectMany(method => method.GetCustomAttributes(typeof(TriggerOnChangeAttribute), true).Cast<TriggerOnChangeAttribute>())
                .Where(attribute => attribute.VerifyStaticExistence)
                .Select(attribute => attribute.TriggerPropertyName);

            propertyNames.Each(VerifyTriggerProperty);
        }

        private void VerifyTriggerProperty(string propertyName)
        {
            if (GetType().GetProperty(propertyName) == null)
            {
                throw new ArgumentException("TriggerChange Property Does Not Exist: " + propertyName);
            }
        }

        #endregion

        #region IDataError Implementation

        //TODO: IMPLEMENT
        public string this[string columnName] => throw new NotImplementedException(); //TODO: IMPLEMENT

        //TODO: IMPLEMENT
        public string Error { get; } //TODO: IMPLEMENT

        #endregion IDataError Implementation

        #region Handle Method dependencies

        private readonly IDictionary<string, List<string>> _methodMap;

        private void TriggerDependentMethods(string name)
        {
            if (!_methodMap.ContainsKey(name)) return;

            _methodMap[name].Each(ExecuteMethod);
        }

        private void ExecuteMethod(string name)
        {
            var memberInfo = GetType().GetMethod(name);
            if (memberInfo == null) return;

            memberInfo.Invoke(this, null);
        }

        #endregion Handle Method dependencies

        #region Handle Command dependencies

        private readonly IDictionary<string, List<string>> _commandMap;

        private void FireChangesOnDependentCommands(string name)
        {
            if (!_commandMap.ContainsKey(name)) return;

            _commandMap[name].Each(RaiseCanExecuteChangedEvent);
        }

        private void CreateCommands() => CommandNames.Each(name => Set(name,
            new DelegateCommand<object>(x => ExecuteCommand(name, x), x => CanExecuteCommand(name, x))));

        private IEnumerable<string> CommandNames => GetType().GetMethods()
            .Where(method => method.Name.StartsWith(ExecutePrefix))
            .Select(method => method.Name.RemovePrefix(ExecutePrefix));

        private void ExecuteCommand(string name, object parameter)
        {
            var methodInfo = GetType().GetMethod(ExecutePrefix + name);
            if (methodInfo == null) return;

            methodInfo.Invoke(this, methodInfo.GetParameters().Length == 1 ? new[] {parameter} : null);
        }

        private bool CanExecuteCommand(string name, object parameter)
        {
            var methodInfo = GetType().GetMethod(CanExecutePrefix + name);
            if (methodInfo == null) return true;

            return (bool) methodInfo.Invoke(this, methodInfo.GetParameters().Length == 1 ? new[] {parameter} : null);
        }

        protected void RaiseCanExecuteChangedEvent(string canExecuteName) => Get<DelegateCommand<object>>(canExecuteName.RemovePrefix(CanExecutePrefix))?.RaiseCanExecuteChanged();

        private string CanExecutePrefix { get; } = ConfigVariables.CanExecutePrefix;

        private static string ExecutePrefix { get; } = ConfigVariables.ExecutePrefix;

        #endregion Handle Command dependencies

        ///// <summary>
        ///// Check if a derived (xaml) class is opened in design mode, as some methods needs different implementation for the design window to function
        ///// </summary>
        ////public bool IsInDesignMode => (bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
        //public static readonly bool IsInDesignModeStatic;

        //static ViewModelBase()
        //{ 
        //    IsInDesignModeStatic = (bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
        //}
    }
}
