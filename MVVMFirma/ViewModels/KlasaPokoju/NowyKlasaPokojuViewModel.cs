using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NowyKlasaPokojuViewModel : JedenViewModel<KlasaPokoju>
    {
        #region Constructor
        public NowyKlasaPokojuViewModel() 
            :base("Klasa pokoju")
        {
            item = new KlasaPokoju();
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get
            {
                return item.Nazwa;
            }
            set
            {
                item.Nazwa = value;
                OnPropertyChanged(() => Nazwa);
            }
        }

        public decimal Doplata
        {
            get
            {
                return item.Doplata;
            }
            set
            {
                item.Doplata = value;
                OnPropertyChanged(() => Doplata);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.KlasaPokoju.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
