using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class ModalViewModel<T> : WorkspaceViewModel
    {
        #region DB
        protected readonly HotelEntities HotelEntities;
        #endregion

        #region List
        private ObservableCollection<T> _list;
        public ObservableCollection<T> List
        {
            get => _list;
            set
            {
                _list = value;
                OnPropertyChanged(() => List);
            }
        }
        #endregion

        #region SelectedItem
        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!object.ReferenceEquals(_selectedItem, value))
                {
                    _selectedItem = value;
                    OnPropertyChanged(() => SelectedItem);
                    ((BaseCommand)SendItemCommand).RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Commands
        public ICommand LoadCommand { get; }
        public ICommand SendItemCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Methods
        public abstract void Load();

        public abstract void SendItem();

        protected virtual bool CanSendItem() => SelectedItem != null;

        protected virtual void Cancel()
        {
            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(x => x.IsActive)
                ?.Close();
        }
        #endregion

        #region Constructor
        protected ModalViewModel()
        {
            HotelEntities = new HotelEntities();
            LoadCommand = new BaseCommand(Load);
            SendItemCommand = new BaseCommand(SendItem, CanSendItem);
            CancelCommand = new BaseCommand(Cancel);
        }
        #endregion
    }
}
