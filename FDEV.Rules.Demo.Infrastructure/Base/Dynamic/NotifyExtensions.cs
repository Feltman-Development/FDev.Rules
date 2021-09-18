using System;
using System.ComponentModel;

namespace FSites.Core.Domain.Dynamic
{
    public static class NotifyExtensions
    {
        public static void Raise(this PropertyChangedEventHandler eventHandler, object source, string propertyName)
            => eventHandler?.Invoke(source, new PropertyChangedEventArgs(propertyName));

        public static void Raise(this EventHandler eventHandler, object source)
            => eventHandler?.Invoke(source, EventArgs.Empty);

        public static void Register(this INotifyPropertyChanged model, string propertyName, Action whenChanged)
            => model.PropertyChanged += (sender, args) => { if (args.PropertyName == propertyName) whenChanged();};
    }
}
