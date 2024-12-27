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

        public string LiczbaDoroslych
        {
            get
            {
                return item.LiczbaDoroslych;
            }
            set
            {
                item.LiczbaDoroslych = value;
                OnPropertyChanged(() => LiczbaDoroslych);
            }
        }

        public string LiczbaDzieci
        {
            get
            {
                return item.LiczbaDzieci;
            }
            set
            {
                item.LiczbaDzieci = value;
                OnPropertyChanged(() => LiczbaDzieci);
            }
        }

        public bool CzyZwierzeta
        {
            get
            {
                return item.CzyZwierzeta;
            }
            set
            {
                item.CzyZwierzeta = value;
                OnPropertyChanged(() => CzyZwierzeta);
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

        public decimal Kwota
        {
            get
            {
                return item.Kwota;
            }
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }

        public IQueryable<KeyAndValue> KlientItems
        {
            get
            {
                return new KlientB(db).GetKlientKeyAndValueItems();
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
                case nameof(LiczbaDoroslych):
                    return !Helper.StringValidator.ContainsOnlyNumbers(LiczbaDoroslych) ? "Prosze wprowadz liczbe dorosłych" : string.Empty;
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
