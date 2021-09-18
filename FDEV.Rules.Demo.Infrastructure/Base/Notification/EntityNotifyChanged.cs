using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FSites.Model.Base
{
    /// <summary>
    /// Base class for very easy INotify implementation. You don't even need a backing field!
    /// Example of property:
    /// public DateTime PayoutDate { get => GetValue'DateTime'(); set => Set(value); }
    /// </summary>
    public abstract class NotifyChanged<TId> : Entity<TId>, INotifyChanged
    {
        public new TId Id { get; }

        #region Property

        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        public bool SuppressPropertyChanged { get; set; }

        protected TProperty GetValue<TProperty>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            return _propertyValues.TryGetValue(propertyName, out object value) ? (TProperty)value : default;
        }

        /// <summary>
        /// Checks if the property in the property backing dictionary is different from <paramref name="value"/>
        /// and sets it accordingly. If the value is updated, a PropertychangedEvent is fired.
        /// </summary>
        /// <returns>True if the field was updated</returns>
        public virtual bool SetValue<TProperty>(TProperty value, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<TProperty>.Default.Equals(value, GetValue<TProperty>(property))) return false;
            _propertyValues[property] = value;
            OnPropertyChanged(property);
            return true;
        }

        /// <summary>
        /// Set(()=> somewhere.Name = value; somewhere.Name, value) 
        /// Advanced case where you rely on another property
        /// </summary>
         public bool SetValue<TProperty>(TProperty value, Action doSet, [CallerMemberName] string property = null)
         {
            if (EqualityComparer<TProperty>.Default.Equals(value, GetValue<TProperty>(property))) return false;
            doSet.Invoke();
            _propertyValues[property] = value;
            OnPropertyChanged(property);
            return true;
         }

        /// <summary>
        /// Use to raise a PropertyChangedEvent for a certain property
        /// </summary>
        /// <param name="selectorExpression">Expression in the form "() => PropertyName"</param>
        public virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> selectorExpression)
        {
            if (selectorExpression == null) throw new ArgumentNullException(nameof(selectorExpression));
            if (!(selectorExpression.Body is MemberExpression body)) throw new ArgumentException("The body must be a member expression");

            OnPropertyChanged(body.Member.Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion

        #region Collectíon

        public bool SuppressCollectionChanged { get; set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void OnCollectionChanged([CallerMemberName] string name = null)
        {
            if (!SuppressCollectionChanged) CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, name));
        }

        public void OnCollectionChanged(NotifyCollectionChangedAction action, IEntity changedItem)
        {
            if (!SuppressCollectionChanged) OnCollectionChanged(action, new List<IEntity> { changedItem });
        }

        public void OnCollectionChanged(NotifyCollectionChangedAction action, IEnumerable<IEntity> entities)
        {
            if (!SuppressCollectionChanged) CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, entities));
        }

        #endregion
    }

}