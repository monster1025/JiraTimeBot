using System;
using System.Windows.Input;

namespace Aeroclub.CL.ASIA.Application.UI.Wpf.Commands
{
    public sealed class RelayCommand : ICommand
    {
        private readonly Action _execute;

        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter = default(object))
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }
    }
}