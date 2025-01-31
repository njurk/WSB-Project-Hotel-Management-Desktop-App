using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class KlienciModalneViewModel : ModalViewModel<KlientForAllView>
    {
        #region DB
        private readonly HotelEntities db;
        #endregion

        #region Constructor
        public KlienciModalneViewModel()
        {
            db = new HotelEntities();
            Load();
        }
        #endregion

        #region Load
        public override void Load()
        {
            var query = db.Klient.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Klient> query)
        {
            var result = query.Select(klient => new KlientForAllView
            {
                IdKlienta = klient.IdKlienta,
                Imie = klient.Imie,
                Nazwisko = klient.Nazwisko,
                Ulica = klient.Ulica,
                NrDomu = klient.NrDomu,
                NrLokalu = klient.NrLokalu,
                KodPocztowy = klient.KodPocztowy,
                Miasto = klient.Miasto,
                Kraj = klient.Kraj.Nazwa,
                Email = klient.Email,
                Telefon = klient.Telefon,
                NIP = klient.NIP
            }).ToList();

            List = new ObservableCollection<KlientForAllView>(result);
        }
        #endregion

        #region Send selected item
        public override void SendItem()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(SelectedItem.IdKlienta);
                Cancel();
            }
        }
        #endregion

        #region Sort and Find
        // tu decydujemy po czym sortować
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Imię", "Nazwisko", "Ulica", "Kod pocztowy", "Miasto",
                "Kraj", "Email", "Telefon", "NIP"
            };
        }

        // tu decydujemy jak sortować
        public override void Sort()
        {
            var query = db.Klient.AsQueryable();

            switch (SortField)
            {
                case "Imię":
                    query = query.OrderBy(k => k.Imie);
                    break;
                case "Nazwisko":
                    query = query.OrderBy(k => k.Nazwisko);
                    break;
                case "Ulica":
                    query = query.OrderBy(k => k.Ulica);
                    break;
                case "Kod pocztowy":
                    query = query.OrderBy(k => k.KodPocztowy);
                    break;
                case "Miasto":
                    query = query.OrderBy(k => k.Miasto);
                    break;
                case "Kraj":
                    query = query.OrderBy(k => k.Kraj.Nazwa);
                    break;
                case "Email":
                    query = query.OrderBy(k => k.Email);
                    break;
                case "Telefon":
                    query = query.OrderBy(k => k.Telefon);
                    break;
                case "NIP":
                    query = query.OrderBy(k => k.NIP);
                    break;
                default:
                    break;
            }
            Reload(query);
        }

        // tu decydujemy po czym wyszukiwać
        public override List<string> GetComboboxFindList()
        {
            return new List<string>
            {
                "Imię", "Nazwisko", "Ulica", "Kod pocztowy", "Miasto",
                "Kraj", "Email", "Telefon", "NIP"
            };
        }

        // tu decydujemy jak wyszukiwać
        public override void Find()
        {
            var query = db.Klient.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Imię":
                        query = query.Where(k => k.Imie.Contains(FindTextBox));
                        break;
                    case "Nazwisko":
                        query = query.Where(k => k.Nazwisko.Contains(FindTextBox));
                        break;
                    case "Ulica":
                        query = query.Where(k => k.Ulica.Contains(FindTextBox));
                        break;
                    case "Kod pocztowy":
                        query = query.Where(k => k.KodPocztowy.Contains(FindTextBox));
                        break;
                    case "Miasto":
                        query = query.Where(k => k.Miasto.Contains(FindTextBox));
                        break;
                    case "Kraj":
                        query = query.Where(k => k.Kraj.Nazwa.Contains(FindTextBox));
                        break;
                    case "Email":
                        query = query.Where(k => k.Email.Contains(FindTextBox));
                        break;
                    case "Telefon":
                        query = query.Where(k => k.Telefon.Contains(FindTextBox));
                        break;
                    case "NIP":
                        query = query.Where(k => k.NIP.Contains(FindTextBox));
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
