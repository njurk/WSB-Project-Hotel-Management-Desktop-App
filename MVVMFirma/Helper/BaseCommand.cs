using MVVMFirma.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MVVMFirma.Helper
{
    public class BaseCommand : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool> _canExecute;

        public BaseCommand(Action command, Func<bool> canExecute = null)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _canExecute = canExecute;
        }

        // wykonuje komendê
        public void Execute(object parameter)
        {
            _command();
        }

        // sprawdza, czy mo¿na ju¿ wykonaæ komendê
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged;
        // wywo³ywane gdy stan CanExecute mo¿e sie zmienic
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public static implicit operator BaseCommand(ReadOnlyCollection<CommandViewModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
