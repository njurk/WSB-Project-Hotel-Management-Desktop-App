using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class PracownicyModalneViewModel : ModalViewModel<PracownikForAllView>
    {
        #region DB
        private readonly HotelEntities db;
        #endregion

        #region Constructor
        public PracownicyModalneViewModel()
        {
            db = new HotelEntities();
            Load();
        }
        #endregion

        #region Load
        public override void Load()
        {
            var query = db.Pracownik.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Pracownik> query)
        {
            var result = query.Select(pracownik => new PracownikForAllView
            {
                IdPracownika = pracownik.IdPracownika,
                StanowiskoNazwa = pracownik.Stanowisko.Nazwa,
                Imie = pracownik.Imie,
                Nazwisko = pracownik.Nazwisko,
                Ulica = pracownik.Ulica,
                NrDomu = pracownik.NrDomu,
                NrLokalu = pracownik.NrLokalu,
                KodPocztowy = pracownik.KodPocztowy,
                Miasto = pracownik.Miasto,
                Kraj = pracownik.Kraj.Nazwa,
                DataUrodzenia = pracownik.DataUrodzenia,
                Email = pracownik.Email,
                Telefon = pracownik.Telefon
            }).ToList();

            List = new ObservableCollection<PracownikForAllView>(result);
        }
        #endregion

        #region Send selected item
        public override void SendItem()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(SelectedItem.IdPracownika);
                Cancel();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Stanowisko", "Imie", "Nazwisko", "Ulica", "Kod pocztowy", "Miasto", "Kraj", "Email", "Telefon" };
        }

        public override void Sort()
        {
            var query = db.Pracownik.AsQueryable();

            switch (SortField)
            {
                case "Stanowisko":
                    query = query.OrderBy(p => p.Stanowisko.Nazwa);
                    break;

                case "Imie":
                    query = query.OrderBy(p => p.Imie);
                    break;

                case "Nazwisko":
                    query = query.OrderBy(p => p.Nazwisko);
                    break;

                case "Ulica":
                    query = query.OrderBy(p => p.Ulica);
                    break;

                case "Kod pocztowy":
                    query = query.OrderBy(p => p.KodPocztowy);
                    break;

                case "Miasto":
                    query = query.OrderBy(p => p.Miasto);
                    break;

                case "Kraj":
                    query = query.OrderBy(p => p.Kraj.Nazwa);
                    break;

                case "Email":
                    query = query.OrderBy(p => p.Email);
                    break;

                case "Telefon":
                    query = query.OrderBy(p => p.Telefon);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Imie", "Nazwisko", "Ulica", "Kod pocztowy", "Stanowisko", "Miasto", "Kraj", "Email", "Telefon" };
        }

        public override void Find()
        {
            var query = db.Pracownik.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Imie":
                        query = query.Where(p => p.Imie.Contains(FindTextBox));
                        break;

                    case "Nazwisko":
                        query = query.Where(p => p.Nazwisko.Contains(FindTextBox));
                        break;

                    case "Stanowisko":
                        query = query.Where(p => p.Stanowisko.Nazwa.Contains(FindTextBox));
                        break;

                    case "Ulica":
                        query = query.Where(p => p.Ulica.Contains(FindTextBox));
                        break;

                    case "Kod pocztowy":
                        query = query.Where(p => p.KodPocztowy.Contains(FindTextBox));
                        break;

                    case "Miasto":
                        query = query.Where(p => p.Miasto.Contains(FindTextBox));
                        break;

                    case "Kraj":
                        query = query.Where(p => p.Kraj.Nazwa.Contains(FindTextBox));
                        break;

                    case "Email":
                        query = query.Where(p => p.Email.Contains(FindTextBox));
                        break;

                    case "Telefon":
                        query = query.Where(p => p.Telefon.Contains(FindTextBox));
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
