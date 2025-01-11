using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyPracownikViewModel : JedenViewModel<Pracownik>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyPracownikViewModel()
            : base("Pracownik")
        {
            db = new HotelEntities();
            item = new Pracownik();
            DataUrodzenia = DateTime.Now;
        }

        public NowyPracownikViewModel(int itemId)
            : base("Edycja pracownika")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Pracownik.FirstOrDefault(p => p.IdPracownika == itemId);

            if (item != null)
            {
                IdRodzajuPracownika = item.IdRodzajuPracownika;
                Imie = item.Imie;
                Nazwisko = item.Nazwisko;
                Ulica = item.Ulica;
                NrDomu = item.NrDomu;
                NrLokalu = item.NrLokalu;
                KodPocztowy = item.KodPocztowy;
                Miasto = item.Miasto;
                IdKraju = item.IdKraju;
                DataUrodzenia = item.DataUrodzenia;
                Email = item.Email;
                Telefon = item.Telefon;
            }
        }
        #endregion

        #region Properties
        public int IdRodzajuPracownika
        {
            get
            {
                return item.IdRodzajuPracownika;
            }
            set
            {
                item.IdRodzajuPracownika = value;
                OnPropertyChanged(() => IdRodzajuPracownika);
            }
        }

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

        public DateTime DataUrodzenia
        {
            get
            {
                return item.DataUrodzenia;
            }
            set
            {
                item.DataUrodzenia = value;
                OnPropertyChanged(() => DataUrodzenia);
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
        #endregion

        #region Items
        public IQueryable<KeyAndValue> RodzajPracownikaItems
        {
            get
            {
                return new RodzajPracownikaB(db).GetRodzajPracownikaKeyAndValueItems();
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
            if (item.IdPracownika == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Pracownik.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Pracownik.FirstOrDefault(f => f.IdPracownika == item.IdPracownika);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("PracownikRefresh");
        }
        #endregion
    }
}
