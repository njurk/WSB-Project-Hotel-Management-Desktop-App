using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyUdogodnieniaKlasPokojuViewModel : JedenViewModel<UdogodnieniaKlasPokoju>
    {
        #region Constructor
        public NowyUdogodnieniaKlasPokojuViewModel()
            :base("Udogodnienie klasy pokoju")
        {
            item = new UdogodnieniaKlasPokoju();
        }
        #endregion

        #region Properties
        public int IdPolaczenia
        {
            get
            {
                return item.IdPolaczenia;
            }
            set
            {
                item.IdPolaczenia = value;
                OnPropertyChanged(() => IdPolaczenia);
            }
        }

        public int IdKlasyPokoju
        {
            get
            {
                return item.IdKlasyPokoju;
            }
            set
            {
                item.IdKlasyPokoju = value;
                OnPropertyChanged(() => IdKlasyPokoju);
            }
        }

        public int IdUdogodnienia
        {
            get
            {
                return item.IdUdogodnienia;
            }
            set
            {
                item.IdUdogodnienia = value;
                OnPropertyChanged(() => IdUdogodnienia);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.UdogodnieniaKlasPokoju.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
