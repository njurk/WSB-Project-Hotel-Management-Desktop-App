using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStanowiskoViewModel : WszystkieViewModel<StanowiskoForAllView>
    {
        #region Constructor
        public WszystkieStanowiskoViewModel()
            : base("Stanowiska")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Stanowisko.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Stanowisko> query)
        {
            var result = query.Select(stanowisko => new StanowiskoForAllView
            {
                IdStanowiska = stanowisko.IdStanowiska,
                Nazwa = stanowisko.Nazwa,
                StawkaGodzinowa = stanowisko.StawkaGodzinowa
            }).ToList();

            List = new ObservableCollection<StanowiskoForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybrane stanowisko:\n{SelectedItem.Nazwa}?"
                    , "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Stanowisko.FirstOrDefault(s => s.IdStanowiska == SelectedItem.IdStanowiska);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Stanowisko.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdStanowiska);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "StanowiskoRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Stawka godzinowa" };
        }

        public override void Sort()
        {
            var query = hotelEntities.Stanowisko.AsQueryable();

            switch (SortField)
            {
                case "Nazwa":
                    query = query.OrderBy(s => s.Nazwa);
                    break;

                case "Stawka godzinowa":
                    query = query.OrderBy(s => s.StawkaGodzinowa);
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
            var query = hotelEntities.Stanowisko.AsQueryable();

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
