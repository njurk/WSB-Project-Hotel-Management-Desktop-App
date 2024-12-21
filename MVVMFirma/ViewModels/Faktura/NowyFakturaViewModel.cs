using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyFakturaViewModel : JedenViewModel<Faktura>
    {
        #region Constructor
        public NowyFakturaViewModel()
            : base("Faktura")
        {
            item = new Faktura();
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

        public int IdRezerwacji
        {
            get
            {
                return item.IdRezerwacji;
            }
            set
            {
                item.IdRezerwacji = value;
                OnPropertyChanged(() => IdRezerwacji);
            }
        }

        public string NrFaktury
        {
            get
            {
                return item.NrFaktury;
            }
            set
            {
                item.NrFaktury = value;
                OnPropertyChanged(() => NrFaktury);
            }
        }

        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        public DateTime DataWystawienia
        {
            get
            {
                return item.DataWystawienia;
            }
            set
            {
                item.DataWystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public DateTime DataSprzedazy
        {
            get
            {
                return item.DataSprzedazy;
            }
            set
            {
                item.DataSprzedazy = value;
                OnPropertyChanged(() => DataSprzedazy);
            }
        }

        public decimal KwotaNetto
        {
            get
            {
                return item.KwotaNetto;
            }
            set
            {
                item.KwotaNetto = value;
                OnPropertyChanged(() => KwotaNetto);
            }
        }

        public decimal VAT
        {
            get
            {
                return item.VAT;
            }
            set
            {
                item.VAT = value;
                OnPropertyChanged(() => VAT);
            }
        }

        public decimal KwotaBrutto
        {
            get
            {
                return item.KwotaBrutto;
            }
            set
            {
                item.KwotaBrutto = value;
                OnPropertyChanged(() => KwotaBrutto);
            }
        }

        public int IdPlatnosci
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

        public DateTime TerminPlatnosci
        {
            get
            {
                return item.TerminPlatnosci;
            }
            set
            {
                item.TerminPlatnosci = value;
                OnPropertyChanged(() => TerminPlatnosci);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Faktura.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
