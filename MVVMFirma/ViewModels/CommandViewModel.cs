using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class CommandViewModel : BaseViewModel
    {
        #region Properties
        public ICommand Command { get; private set; }
        #endregion

        #region Constructor
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            this.DisplayName = displayName;
            this.Command = command;
        }

        public static implicit operator CommandViewModel(ReadOnlyCollection<CommandViewModel> v)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
