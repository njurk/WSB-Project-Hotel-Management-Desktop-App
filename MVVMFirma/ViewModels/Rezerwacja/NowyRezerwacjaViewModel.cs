using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace MVVMFirma.ViewModels
{
    public class NowyRezerwacjaViewModel : JedenViewModel<Rezerwacja>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyRezerwacjaViewModel()
            : base("Rezerwacja")
        {
            item = new Rezerwacja();
            DataRezerwacji = DateTime.Now;
            DataZameldowania = DateTime.Now;
            DataWymeldowania = DateTime.Now.AddDays(1);
            db = new HotelEntities();
        }
        #endregion

        #region Properties
        public int IdKlienta
        {
            get
            {
                return item.IdKlienta;
            }
            set
            {
                item.IdKlienta = value;
                OnPropertyChanged(() => IdKlienta);
            }
        }

        public int IdPokoju
        {
            get
            {
                return item.IdPokoju;
            }
            set
            {
                item.IdPokoju = value;
                OnPropertyChanged(() => IdPokoju);
            }
        }

        public int IdPracownika
        {
            get
            {
                return item.IdPracownika;
            }
            set
            {
                item.IdPracownika = value;
                OnPropertyChanged(() => IdPracownika);
            }
        }

        public string IloscDoroslych
        {
            get
            {
                return item.IloscDoroslych;
            }
            set
            {
                item.IloscDoroslych = value;
                OnPropertyChanged(() => IloscDoroslych);
            }
        }

        public string IloscDzieci
        {
            get
            {
                return item.IloscDzieci;
            }
            set
            {
                item.IloscDzieci = value;
                OnPropertyChanged(() => IloscDzieci);
            }
        }

        public string IloscZwierzat
        {
            get
            {
                return item.IloscZwierzat;
            }
            set
            {
                item.IloscZwierzat = value;
                OnPropertyChanged(() => IloscZwierzat);
            }
        }

        public DateTime DataZameldowania
        {
            get
            {
                return item.DataZameldowania;
            }
            set
            {
                item.DataZameldowania = value;
                OnPropertyChanged(() => DataZameldowania);
            }
        }

        public DateTime DataWymeldowania
        {
            get
            {
                return item.DataWymeldowania;
            }
            set
            {
                item.DataWymeldowania = value;
                OnPropertyChanged(() => DataWymeldowania);
            }
        }

        public DateTime DataRezerwacji
        {
            get
            {
                return item.DataRezerwacji;
            }
            set
            {
                item.DataRezerwacji = value;
                OnPropertyChanged(() => DataRezerwacji);
            }
        }

        public string Uwagi
        {
            get
            {
                return item.Uwagi;
            }
            set
            {
                item.Uwagi = value;
                OnPropertyChanged(() => Uwagi);
            }
        }

        public IQueryable<KeyAndValue> KlientItems
        {
            get
            {
                return new KlientB(db).GetKlientKeyAndValueItems();
            }
        }
        public IQueryable<KeyAndValue> PracownikItems
        {
            get
            {
                return new PracownikB(db).GetPracownikKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> PokojItems
        {
            get
            {
                return new PokojB(db).GetPokojKeyAndValueItems();
            }
        }

        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IloscDoroslych):
                    return !Helper.StringValidator.ContainsOnlyNumbers(IloscDoroslych) ? "Prosze wprowadz liczbe dorosłych" : string.Empty;
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Rezerwacja.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
