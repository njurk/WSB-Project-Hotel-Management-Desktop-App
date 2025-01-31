using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieSposobPlatnosciViewModel : WszystkieViewModel<SposobPlatnosciForAllView>
    {
        #region Constructor
        public WszystkieSposobPlatnosciViewModel()
            : base("Sposoby płatności")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.SposobPlatnosci.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<SposobPlatnosci> query)
        {
            var result = query.Select(sposobplatnosci => new SposobPlatnosciForAllView
            {
                IdSposobuPlatnosci = sposobplatnosci.IdSposobuPlatnosci,
                Nazwa = sposobplatnosci.Nazwa
            }).ToList();

            List = new ObservableCollection<SposobPlatnosciForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybrany sposób płatności:\n{SelectedItem.Nazwa}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.SposobPlatnosci.FirstOrDefault(f => f.IdSposobuPlatnosci == SelectedItem.IdSposobuPlatnosci);
                    if (itemToDelete != null)
                    {
                        hotelEntities.SposobPlatnosci.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdSposobuPlatnosci);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "SposobPlatnosciRefresh")
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
            var query = hotelEntities.SposobPlatnosci.AsQueryable();

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
            var query = hotelEntities.SposobPlatnosci.AsQueryable();

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
