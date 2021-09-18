using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FDEV.Rules.Demo.Domain.Base.Notification;

//TODO: IS SET TO NOT BUILD FOR NOW!
namespace FDEV.Rules.Demo.Domain.Base.Dynamic
{
    /// <summary>
    /// The key ingredients are the use of an attribute ("NotifyOnChangeAttribute") and inversion of dependency - so that the properties, methods and
    /// commands that are dependent on a given property-change, can subscribe to it instead of the property having to know who to notify on change.
    /// Very useful as a base class for dynamic ViewModels in a MVVM solution.
    /// </summary>
    public abstract class DynamicPropertyBase : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Inherit class to gain access the cleanest implementation of INotifyPropertyChanged ever. Write smaller, cleaner and more maintainable viewmodel classes! :-)
        /// Don't let the the Dynamic and Generic stuff put you of from using this base class, it really works and if you are missing any functionality, let me know!
        /// Kind regards, CFE@NYKREDIT.DK
        /// </summary>
        protected DynamicPropertyBase(DynamicPropertyCollectionBase parentCollection = null)
        {
            ParentCollection = parentCollection;
            _propertyMap = MapDependencies<NotifyOnChangeAttribute>(() => GetType().GetProperties());
            _methodMap = MapDependencies<NotifyOnChangeAttribute>(() => GetType().GetMethods().Cast<MemberInfo>().Where(method => !method.Name.StartsWith(CanExecutePrefix)));
            _commandMap = MapDependencies<NotifyOnChangeAttribute>(() => GetType().GetMethods().Cast<MemberInfo>().Where(method => method.Name.StartsWith(CanExecutePrefix)));
            CreateCommands();
            VerifyTriggerProperties();
        }

        public DynamicPropertyCollectionBase ParentCollection { get; set; }

        private readonly Dictionary<string, object> _values = new();

        #region Property getters

        /// <summary>
        /// A long list of 'Getter' overloads to ensure that you can get to your dynamic data in every way you can think of.
        /// Please let all the members stay public or protected, and discover all the possibilities :-)
        /// </summary>

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

        private static IDictionary<string, List<string>> MapDependencies<T>(Func<IEnumerable<MemberInfo>> getInfo) where T : NotifyOnChangeAttribute
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
                .SelectMany(method => method.GetCustomAttributes(typeof(NotifyOnChangeAttribute), true).Cast<NotifyOnChangeAttribute>())
                .Where(attribute => attribute.VerifyStaticExistence)
                .Select(attribute => attribute.TriggerPropertyName);

            propertyNames.Each(VerifyTriggerProperty);
        }

        private void VerifyTriggerProperty(string propertyName)
        {
            if (GetType().GetProperty(propertyName) != null) return;

            throw new ArgumentException("TriggerChange Property Does Not Exist: " + propertyName);
        }

        #endregion

        #region Handle Property dependencies

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
            TriggerDependentCommands(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDictionary<string, List<string>> _propertyMap;

        private void TriggerDependentProperties(string name)
        {
            if (!_propertyMap.ContainsKey(name)) return;

            _propertyMap[name].Each(RaisePropertyChanged);
        }

        #endregion Handle Property dependencies

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

        private void TriggerDependentCommands(string name)
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

        protected void RaiseCanExecuteChangedEvent(string canExecuteName) =>
            Get<DelegateCommand<object>>(canExecuteName.RemovePrefix(CanExecutePrefix))?.RaiseCanExecuteChanged();

        private static string CanExecutePrefix => ConfigVariables.CanExecutePrefix;
        private static string ExecutePrefix => ConfigVariables.ExecutePrefix;

        #endregion Handle Command dependencies
    }

    public interface IDynamicPropertyCollection : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        void Move(int oldIndex, int newIndex);
    }

    public enum ChangeCollectionMode { Insert, Remove, MoveUp, MoveDown };

    public class ChangeCollectionCommand 
    {
        public ChangeCollectionCommand(IDynamicPropertyCollection list, INotifyPropertyChanged item, int index, ChangeCollectionMode mode)
        {
            _properties = list;
            _notification = item;
            _index = index;
            _changeCollectionMode = mode;
        }

        public void Execute()
        {
            if (_changeCollectionMode == ChangeCollectionMode.Insert) Insert();
            else if (_changeCollectionMode == ChangeCollectionMode.Remove) Remove();
            else if (_changeCollectionMode == ChangeCollectionMode.MoveUp) MoveUp();
            else if (_changeCollectionMode == ChangeCollectionMode.MoveDown) MoveDown();
        }

        public void Inverse()
        {
            if (_changeCollectionMode == ChangeCollectionMode.Remove) Insert();
            else if (_changeCollectionMode == ChangeCollectionMode.Insert) Remove();
            else if (_changeCollectionMode == ChangeCollectionMode.MoveDown) MoveUp();
            else if (_changeCollectionMode == ChangeCollectionMode.MoveUp) MoveDown();
        }

        private void Insert() => _properties.Insert(_index, _notification);

        private void Remove() => _properties.Remove(_notification);

        private void MoveUp()
        {
            _index = _properties.IndexOf(_notification);
            _properties.Move(_index, _index - 1);
        }

        private void MoveDown()
        {
            _index = _properties.IndexOf(_notification);
            _properties.Move(_index, _index + 1);
        }

        private int _index;
        private readonly IDynamicPropertyCollection _properties;
        private readonly INotifyPropertyChanged _notification;
        private readonly ChangeCollectionMode _changeCollectionMode;
    }
}


