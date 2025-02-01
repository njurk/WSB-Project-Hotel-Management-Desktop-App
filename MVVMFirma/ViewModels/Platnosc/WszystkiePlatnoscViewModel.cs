using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePlatnoscViewModel : WszystkieViewModel<PlatnoscForAllView>
    {
        #region Constructor
        public WszystkiePlatnoscViewModel()
            : base("Płatności")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Platnosc.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Platnosc> query)
        {
            var result = query.Select(platnosc => new PlatnoscForAllView
            {
                IdPlatnosci = platnosc.IdPlatnosci,
                NrPlatnosci = platnosc.NrPlatnosci,
                NrRezerwacji = platnosc.Rezerwacja.NrRezerwacji,
                SposobPlatnosciNazwa = platnosc.SposobPlatnosci.Nazwa,
                StatusPlatnosciNazwa = platnosc.StatusPlatnosci.Nazwa,
                DataPlatnosci = platnosc.DataPlatnosci,
                Kwota = platnosc.Kwota
            }).ToList();

            List = new ObservableCollection<PlatnoscForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show(
                    $"Czy na pewno chcesz usunąć wybraną płatność:\n{SelectedItem.NrPlatnosci}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Platnosc.FirstOrDefault(p => p.IdPlatnosci == SelectedItem.IdPlatnosci);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Platnosc.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPlatnosci);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "PlatnoscRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer płatności", "Numer rezerwacji", "Sposób płatności", "Status płatności", "Data płatności", "Kwota rosnąco", "Kwota malejąco" };
        }

        public override void Sort()
        {
            var query = hotelEntities.Platnosc.AsQueryable();

            switch (SortField)
            {
                case "Numer płatności":
                    query = query.OrderBy(p => p.NrPlatnosci);
                    break;

                case "Numer rezerwacji":
                    query = query.OrderBy(p => p.Rezerwacja.NrRezerwacji);
                    break;

                case "Sposób płatności":
                    query = query.OrderBy(p => p.SposobPlatnosci.Nazwa);
                    break;

                case "Status płatności":
                    query = query.OrderBy(p => p.StatusPlatnosci.Nazwa);
                    break;

                case "Data płatności":
                    query = query.OrderByDescending(p => p.DataPlatnosci);
                    break;

                case "Kwota rosnąco":
                    query = query.OrderBy(p => p.Kwota);
                    break;

                case "Kwota malejąco":
                    query = query.OrderByDescending(p => p.Kwota);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer płatności", "Numer rezerwacji", "Sposób płatności", "Status płatności" };
        }

        public override void Find()
        {
            var query = hotelEntities.Platnosc.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Numer płatności":
                        query = query.Where(p => p.NrPlatnosci.Contains(FindTextBox));
                        break;

                    case "Numer rezerwacji":
                        query = query.Where(p => p.Rezerwacja.NrRezerwacji.Contains(FindTextBox));
                        break;

                    case "Sposób płatności":
                        query = query.Where(p => p.SposobPlatnosci.Nazwa.Contains(FindTextBox));
                        break;

                    case "Status płatności":
                        query = query.Where(p => p.StatusPlatnosci.Nazwa.Contains(FindTextBox));
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
