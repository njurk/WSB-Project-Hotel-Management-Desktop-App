using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Collections.Generic;
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

        #region Sort and Find
        // sortowanie
        public string SortField { get; set; }
        public List<string> SortComboboxItems
        {
            get
            {
                return GetComboboxSortList();
            }
        }
        public abstract List<string> GetComboboxSortList();
        private BaseCommand _SortCommand; // komenda która będzie wywołana po naciśnięciu przycisku Sortuj w sortowaniu Generic.xaml
        public ICommand SortCommand
        {
            get
            {
                if (_SortCommand == null)
                    _SortCommand = new BaseCommand(() => Sort());
                return _SortCommand;
            }
        }
        public abstract void Sort();

        // filtrowanie
        public string FindField { get; set; }
        public List<string> FindComboboxItems
        {
            get
            {
                return GetComboboxFindList();
            }
        }
        public abstract List<string> GetComboboxFindList();
        public string FindTextBox { get; set; }
        private BaseCommand _FindCommand; // komenda która będzie wywołana po naciśnięciu przycisku Szukaj
        public ICommand FindCommand
        {
            get
            {
                if (_FindCommand == null)
                    _FindCommand = new BaseCommand(() => { Load(); Find(); }); // przed każdym wyszukaniem lista załadowuje się na nowo
                return _FindCommand;
            }
        }
        public abstract void Find();
        #endregion
    }
}
