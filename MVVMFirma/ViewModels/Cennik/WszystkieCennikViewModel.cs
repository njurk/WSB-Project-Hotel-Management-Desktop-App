using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieCennikViewModel : WszystkieViewModel<CennikForAllView>
    {
        #region Constructor
        public WszystkieCennikViewModel()
            : base("Cenniki")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Cennik.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Cennik> query)
        {
            var result = query.Select(cennik => new CennikForAllView
            {
                IdCennika = cennik.IdCennika,
                KlasaPokojuNazwa = cennik.KlasaPokoju.Nazwa,
                TypPokojuNazwa = cennik.TypPokoju.Nazwa,
                CenaDorosly = cennik.CenaDorosly,
                CenaDziecko = cennik.CenaDziecko,
                CenaZwierzeta = cennik.CenaZwierzeta
            }).ToList();

            List = new ObservableCollection<CennikForAllView>(result);
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany cennik?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Cennik.Remove(hotelEntities.Cennik.FirstOrDefault(c => c.IdCennika == SelectedItem.IdCennika));
                hotelEntities.SaveChanges();
                Load();
            }
        }

        // w celu edycji wybranego rekordu wysyłana jest wiadomość zawierająca jego ID
        // odbiera i obsługuje ją metoda open() w klasie MainWindowViewModel
        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdCennika);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "CennikRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        // tu decydujemy po czym sortować
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Klasa pokoju", "Typ pokoju", "Ceny rosnąco", "Ceny malejąco"};
        }

        // tu decydujemy jak sortować
        public override void Sort()
        {
            var query = hotelEntities.Cennik.AsQueryable();

            switch (SortField)
            {
                case "Klasa pokoju":
                    query = query.OrderBy(c => c.KlasaPokoju.Nazwa);
                    break;

                case "Typ pokoju":
                    query = query.OrderBy(c => c.TypPokoju.Nazwa);
                    break;

                case "Ceny rosnąco":
                    query = query.OrderBy(c => c.CenaDorosly);
                    break;

                case "Ceny malejąco":
                    query = query.OrderBy(c => c.CenaDorosly);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        // tu decydujemy po czym wyszukiwać
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Klasa pokoju", "Typ pokoju" };
        }

        // tu decydujemy jak wyszukiwać
        public override void Find()
        {
            var query = hotelEntities.Cennik.AsQueryable();

            switch (FindField)
            {
                case "Klasa pokoju":
                    query = query.Where(c => c.KlasaPokoju.Nazwa.Contains(FindTextBox));
                    break;

                case "Typ pokoju":
                    query = query.Where(c => c.TypPokoju.Nazwa.Contains(FindTextBox));
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        #endregion
    }
}
