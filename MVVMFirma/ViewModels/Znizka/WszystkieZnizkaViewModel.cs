using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieZnizkaViewModel : WszystkieViewModel<ZnizkaForAllView>
    {
        #region Constructor
        public WszystkieZnizkaViewModel()
            : base("Zniżki")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Znizka.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Znizka> query)
        {
            var result = query.Select(znizka => new ZnizkaForAllView
            {
                IdZnizki = znizka.IdZnizki,
                Nazwa = znizka.Nazwa,
                Wartosc = znizka.Wartosc
            }).ToList();

            List = new ObservableCollection<ZnizkaForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybraną zniżkę:\n{SelectedItem.Nazwa} {SelectedItem.Wartosc}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Znizka.FirstOrDefault(z => z.IdZnizki == SelectedItem.IdZnizki);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Znizka.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdZnizki);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "ZnizkaRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Wartość" };
        }

        public override void Sort()
        {
            var query = hotelEntities.Znizka.AsQueryable();

            //  sortowanie lokalne z powodu konieczności parsowania
            var result = query.Select(z => new ZnizkaForAllView
            {
                IdZnizki = z.IdZnizki,
                Nazwa = z.Nazwa,
                Wartosc = z.Wartosc
            }).AsEnumerable();

            switch (SortField)
            {
                case "Nazwa":
                    result = result.OrderBy(z => z.Nazwa);
                    break;

                case "Wartość":
                    result = result.OrderBy(z => int.TryParse(z.Wartosc.ToString(), out var wartoscInt) ? wartoscInt : 0);
                    break;

                default:
                    break;
            }

            List = new ObservableCollection<ZnizkaForAllView>(result);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa", "Wartość" };
        }

        public override void Find()
        {
            var query = hotelEntities.Znizka.AsQueryable();

            // wyszukiwanie lokalne z powodu konieczności parsowania
            var result = query.Select(z => new ZnizkaForAllView
            {
                IdZnizki = z.IdZnizki,
                Nazwa = z.Nazwa,
                Wartosc = z.Wartosc
            }).AsEnumerable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Nazwa":
                        result = result.Where(z => z.Nazwa.Contains(FindTextBox));
                        break;

                    case "Wartość":
                        if (int.TryParse(FindTextBox, out var wartoscInt))
                        {
                            result = result.Where(z => int.TryParse(z.Wartosc.ToString(), out var zniżkaWartoscInt) && zniżkaWartoscInt == wartoscInt);
                        }
                        break;

                    default:
                        break;
                }
            }

            List = new ObservableCollection<ZnizkaForAllView>(result);
        }
        #endregion
    }
}
