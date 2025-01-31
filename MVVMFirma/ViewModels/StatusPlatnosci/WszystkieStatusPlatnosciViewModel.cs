using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusPlatnosciViewModel : WszystkieViewModel<StatusPlatnosciForAllView>
    {
        #region Constructor
        public WszystkieStatusPlatnosciViewModel()
            : base("Statusy płatności")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.StatusPlatnosci.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<StatusPlatnosci> query)
        {
            var result = query.Select(statusplatnosci => new StatusPlatnosciForAllView
            {
                IdStatusuPlatnosci = statusplatnosci.IdStatusuPlatnosci,
                Nazwa = statusplatnosci.Nazwa
            }).ToList();

            List = new ObservableCollection<StatusPlatnosciForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybrany status płatności:\n{SelectedItem.Nazwa}?"
                    , "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.StatusPlatnosci.FirstOrDefault(s => s.IdStatusuPlatnosci == SelectedItem.IdStatusuPlatnosci);
                    if (itemToDelete != null)
                    {
                        hotelEntities.StatusPlatnosci.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdStatusuPlatnosci);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "StatusPlatnosciRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa" };
        }

        public override void Sort()
        {
            var query = hotelEntities.StatusPlatnosci.AsQueryable();

            switch (SortField)
            {
                case "Nazwa":
                    query = query.OrderBy(s => s.Nazwa);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa" };
        }

        public override void Find()
        {
            var query = hotelEntities.StatusPlatnosci.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Nazwa":
                        query = query.Where(s => s.Nazwa.Contains(FindTextBox));
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