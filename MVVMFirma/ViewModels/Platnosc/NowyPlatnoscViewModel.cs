using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyPlatnoscViewModel : JedenViewModel<Platnosc>
    {
        #region Constructor
        public NowyPlatnoscViewModel()
            :base("Płatność")
        {
            item = new Platnosc();
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

        public int IdSposobuPlatnosci
        {
            get
            {
                return item.IdSposobuPlatnosci;
            }
            set
            {
                item.IdSposobuPlatnosci = value;
                OnPropertyChanged(() => IdSposobuPlatnosci);
            }
        }

        public int IdStatusuPlatnosci
        {
            get
            {
                return item.IdStatusuPlatnosci;
            }
            set
            {
                item.IdStatusuPlatnosci = value;
                OnPropertyChanged(() => IdStatusuPlatnosci);
            }
        }

        public DateTime? DataPlatnosci
        {
            get
            {
                return item.DataPlatnosci;
            }
            set
            {
                item.DataPlatnosci = value;
                OnPropertyChanged(() => DataPlatnosci);
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
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Platnosc.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion

    }
}
