using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyKlientViewModel : JedenViewModel<Klient>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyKlientViewModel()
            : base("Klient")
        {
            db = new HotelEntities();
            item = new Klient();
        }

        public NowyKlientViewModel(int itemId)
            : base("Edycja klienta")
        {
            db = new HotelEntities();
            item = db.Klient.FirstOrDefault(k => k.IdKlienta == itemId);

            if (item != null)
            {
                Imie = item.Imie;
                Nazwisko = item.Nazwisko;
                Ulica = item.Ulica;
                NrDomu = item.NrDomu;
                NrLokalu = item.NrLokalu;
                KodPocztowy = item.KodPocztowy;
                Miasto = item.Miasto;
                IdKraju = item.IdKraju;
                Email = item.Email;
                Telefon = item.Telefon;
                NIP = item.NIP;
            }
        }
        #endregion

        #region Properties
        public string Imie
        {
            get
            {
                return item.Imie;
            }
            set
            {
                item.Imie = value;
                OnPropertyChanged(() => Imie);
            }
        }

        public string Nazwisko
        {
            get
            {
                return item.Nazwisko;
            }
            set
            {
                item.Nazwisko = value;
                OnPropertyChanged(() => Nazwisko);
            }
        }

        public string Ulica
        {
            get
            {
                return item.Ulica;
            }
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Ulica);
            }
        }

        public string NrDomu
        {
            get
            {
                return item.NrDomu;
            }
            set
            {
                item.NrDomu = value;
                OnPropertyChanged(() => NrDomu);
            }
        }

        public string NrLokalu
        {
            get
            {
                return item.NrLokalu;
            }
            set
            {
                item.NrLokalu = value;
                OnPropertyChanged(() => NrLokalu);
            }
        }

        public string KodPocztowy
        {
            get
            {
                return item.KodPocztowy;
            }
            set
            {
                item.KodPocztowy = value;
                OnPropertyChanged(() => KodPocztowy);
            }
        }

        public string Miasto
        {
            get
            {
                return item.Miasto;
            }
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => Miasto);
            }
        }

        public int IdKraju
        {
            get
            {
                return item.IdKraju;
            }
            set
            {
                item.IdKraju = value;
                OnPropertyChanged(() => IdKraju);
            }
        }

        public string Email
        {
            get
            {
                return item.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }

        public string Telefon
        {
            get
            {
                return item.Telefon;
            }
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Telefon);
            }
        }

        public string NIP
        {
            get
            {
                return item.NIP;
            }
            set
            {
                item.NIP = value;
                OnPropertyChanged(() => NIP);
            }
        }

        public IQueryable<KeyAndValue> KrajItems
        {
            get
            {
                return new KrajB(db).GetKrajKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdKlienta == 0) // brak ID = insert
            {
                db.Klient.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Klient.FirstOrDefault(f => f.IdKlienta == item.IdKlienta);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("KlientRefresh");
        }
        #endregion
    }
}
