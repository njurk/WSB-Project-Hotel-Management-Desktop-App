using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieFakturaViewModel : WszystkieViewModel<FakturaForAllView>
    {
        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Faktura.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Faktura> query)
        {
            var result = query.Select(faktura => new FakturaForAllView
            {
                IdFaktury = faktura.IdFaktury,
                NrFaktury = faktura.NrFaktury,
                NrRezerwacji = faktura.Rezerwacja.NrRezerwacji,
                NIP = faktura.NIP,
                KlientImie = faktura.Rezerwacja.Klient.Imie,
                KlientNazwisko = faktura.Rezerwacja.Klient.Nazwisko,
                DataWystawienia = faktura.DataWystawienia,
                DataSprzedazy = faktura.DataSprzedazy,
                KwotaNetto = faktura.KwotaNetto,
                VAT = faktura.VAT.Stawka,
                KwotaBrutto = faktura.KwotaBrutto,
                TerminPlatnosci = faktura.TerminPlatnosci,
                Zaplacono = hotelEntities.Platnosc
                    .Where(p => p.IdRezerwacji == faktura.IdRezerwacji)
                    .Sum(p => (decimal?)p.Kwota) ?? 0,
                Opis = faktura.Opis
            }).ToList();

            List = new ObservableCollection<FakturaForAllView>(result);
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną fakturę:\n" + SelectedItem.NrFaktury, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Faktura.Remove(hotelEntities.Faktura.FirstOrDefault(f => f.IdFaktury == SelectedItem.IdFaktury));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdFaktury);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku
        // Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "FakturaRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Constructor
        public WszystkieFakturaViewModel()
            : base("Faktury")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Sort and Find
        // tu decydujemy po czym sortować
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer faktury", "Numer rezerwacji", "NIP", "Imie klienta", "Nazwisko klienta", "Data wystawienia", "Data sprzedaży", 
                "Kwota rosnąco", "Kwota malejąco", "Termin płatności", "Zapłacono" };
        }

        // tu decydujemy jak sortować
        public override void Sort()
        {
            var query = hotelEntities.Faktura.AsQueryable();

            switch (SortField)
            {
                case "Numer faktury":
                    query = query.OrderBy(f => f.NrFaktury);
                    break;

                case "Numer rezerwacji":
                    query = query.OrderBy(f => f.Rezerwacja.NrRezerwacji);
                    break;

                case "NIP":
                    query = query.OrderBy(f => f.NIP);
                    break;

                case "Imie klienta":
                    query = query.OrderBy(f => f.Rezerwacja.Klient.Imie);
                    break;

                case "Nazwisko klienta":
                    query = query.OrderBy(f => f.Rezerwacja.Klient.Nazwisko);
                    break;

                case "Data wystawienia":
                    query = query.OrderByDescending(f => f.DataWystawienia);
                    break;

                case "Data sprzedaży":
                    query = query.OrderByDescending(f => f.DataSprzedazy);
                    break;

                case "Kwota rosnąco":
                    query = query.OrderBy(f => f.KwotaBrutto);
                    break;

                case "Kwota malejąco":
                    query = query.OrderByDescending(f => f.KwotaBrutto);
                    break;

                case "Termin płatności":
                    query = query.OrderByDescending(f => f.TerminPlatnosci);
                    break;

                case "Zapłacono":
                    query = query.OrderBy(f => hotelEntities.Platnosc
                        .Where(p => p.IdRezerwacji == f.IdRezerwacji)
                        .Sum(p => (decimal?)p.Kwota) ?? 0);
                    break;

                default:
                    break;
            }
            Reload(query);
        }

        // tu decydujemy po czym wyszukiwać
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer faktury", "Numer rezerwacji", "NIP", "Imie klienta", "Nazwisko klienta", "Opis"};
        }

        // tu decydujemy jak wyszukiwać
        public override void Find()
        {
            var query = hotelEntities.Faktura.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Numer faktury":
                        query = query.Where(f => f.NrFaktury.Contains(FindTextBox));
                        break;

                    case "Numer rezerwacji":
                        query = query.Where(f => f.Rezerwacja.NrRezerwacji.Contains(FindTextBox));
                        break;

                    case "NIP":
                        query = query.Where(f => f.NIP.Contains(FindTextBox));
                        break;

                    case "Imie klienta":
                        query = query.Where(f => f.Rezerwacja.Klient.Imie.Contains(FindTextBox));
                        break;

                    case "Nazwisko klienta":
                        query = query.Where(f => f.Rezerwacja.Klient.Nazwisko.Contains(FindTextBox));
                        break;

                    case "Opis":
                        query = query.Where(f => f.Opis.Contains(FindTextBox));
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
