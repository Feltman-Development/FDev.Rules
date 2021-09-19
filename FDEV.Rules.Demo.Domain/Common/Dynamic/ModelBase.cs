using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace FDEV.Rules.Demo.Domain.Common.Dynamic
{
    [DataContract(Namespace = "")]
    public class ModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged related

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Checks if the property in <paramref name="field"/> is different from <paramref name="value"/>
        /// and sets it accordingly. If the value is updated, a PropertychangedEvent is fired
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="field">The variable that holds the current value</param>
        /// <param name="value">The new value</param>
        /// <param name="name"></param>
        /// <returns>True if the field was updated</returns>
        protected virtual bool SetProp<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        /// <summary>
        /// Use to raise a PropertyChangedEvent for a certain property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectorExpression">Expression in the form "() => PropertyName"</param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null) throw new ArgumentNullException(nameof(selectorExpression));

            var body = selectorExpression.Body as MemberExpression;
            if (body == null) throw new ArgumentException("The body must be a member expression");

            OnPropertyChanged(body.Member.Name);
        }

        protected bool Set<T>(ref T field, T value, Expression<Func<T>> selectorExpression)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;

            OnPropertyChanged(selectorExpression); return true;
        }

        #endregion
    }

    public class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // SetField (Name, value); // where there is a data member
        protected bool SetProp<T>(ref T field, T value, [CallerMemberName] String property = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(property);
            return true;
        }

        // SetField(()=> somewhere.Name = value; somewhere.Name, value) 
        // Advanced case where you rely on another property
        protected bool SetProp<T>(ref T field, T value, Action DoSet, [CallerMemberName] String property = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            DoSet.Invoke();
            field = value;
            RaisePropertyChanged(property);
            return true;
        }

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class NotificationBase<T> : NotificationBase where T : class, new()
    {
        private readonly T _this;
        public static implicit operator T(NotificationBase<T> thing) { return thing._this; }

        public NotificationBase(T thing = null)
        {
            _this = thing ?? new T();
        }
    }
}
