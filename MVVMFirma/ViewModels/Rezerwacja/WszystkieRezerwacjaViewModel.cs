using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRezerwacjaViewModel : WszystkieViewModel<RezerwacjaForAllView>
    {
        #region Constructor
        public WszystkieRezerwacjaViewModel()
            : base("Rezerwacje")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Rezerwacja.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Rezerwacja> query)
        {
            var result = query.Select(rezerwacja => new RezerwacjaForAllView
            {
                IdRezerwacji = rezerwacja.IdRezerwacji,
                NrRezerwacji = rezerwacja.NrRezerwacji,
                KlientImie = rezerwacja.Klient.Imie,
                KlientNazwisko = rezerwacja.Klient.Nazwisko,
                NrPokoju = rezerwacja.Pokoj.NrPokoju,
                LiczbaDoroslych = rezerwacja.LiczbaDoroslych,
                LiczbaDzieci = rezerwacja.LiczbaDzieci,
                CzyZwierzeta = rezerwacja.CzyZwierzeta,
                DataRezerwacji = rezerwacja.DataRezerwacji,
                DataZameldowania = rezerwacja.DataZameldowania,
                DataWymeldowania = rezerwacja.DataWymeldowania,
                Kwota = rezerwacja.Kwota,
                CzyZaplacona = hotelEntities.Platnosc
                    .Where(p => p.IdRezerwacji == rezerwacja.IdRezerwacji)
                    .Sum(p => (decimal?)p.Kwota) >= rezerwacja.Kwota ? true : false,
                Uwagi = rezerwacja.Uwagi,
                Znizka = rezerwacja.Znizka.Wartosc
            }).ToList();

            List = new ObservableCollection<RezerwacjaForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybraną rezerwację:\n{SelectedItem.NrRezerwacji}?"
                    , "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == SelectedItem.IdRezerwacji);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Rezerwacja.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdRezerwacji);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "RezerwacjaRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer rezerwacji", "Imie", "Nazwisko", "Numer pokoju", "Liczba dorosłych", "Liczba dzieci", "Zwierzęta", "Data rezerwacji", "Data zameldowania", "Data wymeldowania", "Kwota rosnąco", "Kwota malejąco", "Zapłacone","Zniżki" };
        }

        public override void Sort()
        {
            var query = hotelEntities.Rezerwacja.AsQueryable();

            switch (SortField)
            {
                case "Numer rezerwacji":
                    query = query.OrderBy(r => r.NrRezerwacji);
                    break;

                case "Imie":
                    query = query.OrderBy(r => r.Klient.Imie);
                    break;

                case "Nazwisko":
                    query = query.OrderBy(r => r.Klient.Nazwisko);
                    break;

                case "Numer pokoju":
                    query = query.OrderBy(r => r.Pokoj.NrPokoju);
                    break;

                case "Liczba dorosłych":
                    query = query.OrderBy(r => r.LiczbaDoroslych);
                    break;

                case "Liczba dzieci":
                    query = query.OrderBy(r => r.LiczbaDzieci);
                    break;

                case "Zwierzęta":
                    query = query.OrderByDescending(r => r.CzyZwierzeta);
                    break;

                case "Data rezerwacji":
                    query = query.OrderByDescending(r => r.DataRezerwacji);
                    break;

                case "Data zameldowania":
                    query = query.OrderByDescending(r => r.DataZameldowania);
                    break;

                case "Data wymeldowania":
                    query = query.OrderByDescending(r => r.DataWymeldowania);
                    break;

                case "Kwota rosnąco":
                    query = query.OrderBy(r => r.Kwota);
                    break;

                case "Kwota malejąco":
                    query = query.OrderByDescending(r => r.Kwota);
                    break;

                case "Zapłacone":
                    query = query.OrderByDescending(r => hotelEntities.Platnosc
                        .Where(p => p.IdRezerwacji == r.IdRezerwacji)
                        .Sum(p => (decimal?)p.Kwota) >= r.Kwota);
                    break;

                case "Zniżki":
                    query = query.OrderByDescending(r => r.Znizka.Wartosc);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer rezerwacji", "Imie", "Nazwisko", "Numer pokoju" };
        }

        public override void Find()
        {
            var query = hotelEntities.Rezerwacja.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Numer rezerwacji":
                        query = query.Where(r => r.NrRezerwacji.Contains(FindTextBox));
                        break;

                    case "Imie":
                        query = query.Where(r => r.Klient.Imie.Contains(FindTextBox));
                        break;

                    case "Nazwisko":
                        query = query.Where(r => r.Klient.Nazwisko.Contains(FindTextBox));
                        break;

                    case "Numer pokoju":
                        query = query.Where(r => r.Pokoj.NrPokoju.Contains(FindTextBox));
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