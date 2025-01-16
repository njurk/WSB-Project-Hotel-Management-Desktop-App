using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class ModalViewModel<T> : WorkspaceViewModel
    {
        #region DB
        protected readonly HotelEntities hotelEntities;
        #endregion

        #region Item
        protected T item;
        #endregion

        #region List
        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
        #endregion

        #region Commands
        private BaseCommand _LoadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => Load());
                return _LoadCommand;
            }
        }
        #endregion

        #region Selected Item
        private T _SelectedItem;
        public T SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        #endregion

        #region Helpers
        public abstract void Load();
        #endregion

        #region Constructor
        public ModalViewModel()
        { 
            hotelEntities = new HotelEntities();
        }
        #endregion
    }
}
