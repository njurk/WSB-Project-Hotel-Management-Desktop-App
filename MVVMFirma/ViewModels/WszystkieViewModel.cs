using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class WszystkieViewModel<T> : WorkspaceViewModel
    {
        #region DB
        protected readonly HotelEntities hotelEntities;
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

        private BaseCommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                    _AddCommand = new BaseCommand(() => Add());
                return _AddCommand;
            }
        }

        private BaseCommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                    _DeleteCommand = new BaseCommand(() => Delete(), CanDelete);
                return _DeleteCommand;
            }
        }

        private BaseCommand _EditCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                    _EditCommand = new BaseCommand(() => Edit(), CanEdit);
                return _EditCommand;
            }
        }
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

        #region Selected Item
        private T _SelectedItem;
        public T SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(() => SelectedItem);
                (DeleteCommand as BaseCommand)?.RaiseCanExecuteChanged();
                (EditCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Helpers
        private void Add()
        {
            Messenger.Default.Send(DisplayName + "Add");
        }

        private bool CanDelete()
        {
            return SelectedItem != null;
        }

        private bool CanEdit()
        {
            return SelectedItem != null;
        }
        public abstract void Delete();
        public abstract void Edit();
        public abstract void Load();
        #endregion

        #region Constructor
        public WszystkieViewModel(string displayName)
        {
            base.DisplayName = displayName;
            hotelEntities = new HotelEntities();
        }
        #endregion

        #region Sort and Filter
        public string SortField { get; set; }
        public List<string> SortComboboxItems
        {
            get
            {
                return GetComboboxSortList();
            }
        }
        public abstract List<string> GetComboboxSortList();
        #endregion
    }
}
