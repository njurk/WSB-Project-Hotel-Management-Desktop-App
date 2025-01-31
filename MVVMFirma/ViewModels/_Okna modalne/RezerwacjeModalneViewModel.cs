using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class RezerwacjeModalneViewModel : ModalViewModel<RezerwacjaForAllView>
    {
        #region DB
        private readonly HotelEntities db;
        private bool _bezFaktury;
        #endregion

        #region Constructor
        public RezerwacjeModalneViewModel()
        {
            db = new HotelEntities();
            Load();
        }

        // konstruktor okna modalnego dla NowyFakturaViewModel - wyświetlają się tylko rezerwacje, które jeszcze nie mają wystawionej faktury
        public RezerwacjeModalneViewModel(bool bezFaktury)
        {
            db = new HotelEntities();
            _bezFaktury = bezFaktury;
            Load();
        }
        #endregion

        #region Load
        public override void Load()
        {
            var query = db.Rezerwacja.AsQueryable();
            if (_bezFaktury)
            {
                query = query.Where(r => !db.Faktura.Any(f => f.IdRezerwacji == r.IdRezerwacji));
            }
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
                CzyZaplacona = (db.Platnosc
                    .Where(p => p.IdRezerwacji == rezerwacja.IdRezerwacji)
                    .Sum(p => (decimal?)p.Kwota) ?? 0) >= rezerwacja.Kwota,
                Uwagi = rezerwacja.Uwagi,
                Znizka = rezerwacja.Znizka.Wartosc
            }).ToList();

            List = new ObservableCollection<RezerwacjaForAllView>(result);
        }

        #endregion

        #region Send selected item
        public override void SendItem()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(SelectedItem.IdRezerwacji);
                Cancel();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer rezerwacji", "Imie", "Nazwisko", "Numer pokoju", "Liczba dorosłych", "Liczba dzieci", "Zwierzęta", "Data rezerwacji", "Data zameldowania", "Data wymeldowania", "Kwota", "Zapłacone", "Zniżki" };
        }

        public override void Sort()
        {
            var query = db.Rezerwacja.AsQueryable();

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
                    query = query.OrderBy(r => r.DataRezerwacji);
                    break;

                case "Data zameldowania":
                    query = query.OrderBy(r => r.DataZameldowania);
                    break;

                case "Data wymeldowania":
                    query = query.OrderBy(r => r.DataWymeldowania);
                    break;

                case "Kwota":
                    query = query.OrderBy(r => r.Kwota);
                    break;

                case "Zapłacone":
                    query = query.OrderByDescending(r => db.Platnosc
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
            var query = db.Rezerwacja.AsQueryable();

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
