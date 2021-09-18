using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FDEV.Rules.Demo.Domain.Base.Notification
{
    /// <summary>
    /// Interface for controlling property and collection notification on entities
    /// </summary>
    public interface INotifyChanged : INotifyPropertyChanged, INotifyCollectionChanged
    {
        bool SuppressPropertyChanged { get; set; }

        void OnPropertyChanged(string property);

        void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> selectorExpression);

        bool SetValue<TProperty>(TProperty value, [CallerMemberName] string name = null);

        bool SetValue<TProperty>(TProperty value, Action doSet, [CallerMemberName] string property = null);

        bool SuppressCollectionChanged { get; set; }

        void OnCollectionChanged(NotifyCollectionChangedAction action, IEntity changedItem);

        void OnCollectionChanged(NotifyCollectionChangedAction action, IEnumerable<IEntity> entities);
    }
}


