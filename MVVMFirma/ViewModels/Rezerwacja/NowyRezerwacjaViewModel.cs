using MVVMFirma.Models.Entities;
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
        #region Constructor
        public NowyRezerwacjaViewModel()
            : base("Rezerwacja")
        {
            item = new Rezerwacja();
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

        public int? IdPlatnosci
        {
            get
            {
                return item.IdPlatnosci;
            }
            set
            {
                item.IdPlatnosci = value;
                OnPropertyChanged(() => IdPlatnosci);
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
