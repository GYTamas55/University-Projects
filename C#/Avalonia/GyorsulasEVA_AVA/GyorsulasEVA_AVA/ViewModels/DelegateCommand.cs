using System;
using Avalonia.Input;
using System.ComponentModel;
using System.Windows.Input;


namespace GyorsulasEVA_AVA.ViewModels
{
    //Ez kell a view és a model közötti kommunikáciohoz
    public class DelegateCommand : ICommand
    {
        //Amit futtatni akarok
        //Action csinálja azt amit megkap
        private readonly Action<object?> _execute;
        
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }
        //Futtathatjuk?
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        //Futtassuk
        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled");
            }

            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
