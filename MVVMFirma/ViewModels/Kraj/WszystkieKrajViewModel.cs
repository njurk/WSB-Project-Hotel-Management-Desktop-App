using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKrajViewModel : WszystkieViewModel<KrajForAllView>
    {
        #region Constructor
        public WszystkieKrajViewModel()
            : base("Kraje")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Kraj.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Kraj> query)
        {
            var result = query.Select(kraj => new KrajForAllView
            {
                IdKraju = kraj.IdKraju,
                Nazwa = kraj.Nazwa
            }).ToList();

            List = new ObservableCollection<KrajForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybrany kraj: {SelectedItem.Nazwa}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Kraj.FirstOrDefault(f => f.IdKraju == SelectedItem.IdKraju);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Kraj.Remove(itemToDelete);
                        hotelEntities.SaveChanges();
                        Load();
                    }
                }
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdKraju);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "KrajRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        // tu decydujemy po czym sortować
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa" };
        }

        // tu decydujemy jak sortować
        public override void Sort()
        {
            var query = hotelEntities.Kraj.AsQueryable();

            switch (SortField)
            {
                case "Nazwa":
                    query = query.OrderBy(k => k.Nazwa);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        // tu decydujemy po czym wyszukiwać
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa" };
        }

        // tu decydujemy jak wyszukiwać
        public override void Find()
        {
            var query = hotelEntities.Kraj.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Nazwa":
                        query = query.Where(k => k.Nazwa.Contains(FindTextBox));
                        break;

                    default:
                        break;
                }
            }

            Reload(query);
        }
        #endregion
    }
}
