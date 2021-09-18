using System;
using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Base.Dynamic
{
    /// <summary>
    /// Generic delegation of commands. Primarily for use in MVVM design.
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {
        /// <inheritdoc />
        public DelegateCommand(Action<T> execute) : this(execute, _ => true)
        {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public void Execute(object parameter) => _execute((T) parameter);
        private readonly Action<T> _execute;

        public bool CanExecute(object parameter) => _canExecute((T)parameter);
        private readonly Predicate<T> _canExecute;

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged.Raise(this);

        public string CommandName { get; }
        public Dictionary<string, object> Parameters { get; }
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action execute) : base(execute != null ? x => execute() : null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute) : base(execute != null ? x => execute() : null, canExecute != null ? x => canExecute() : (Predicate<object>)null)
        {
        }
    }
}
