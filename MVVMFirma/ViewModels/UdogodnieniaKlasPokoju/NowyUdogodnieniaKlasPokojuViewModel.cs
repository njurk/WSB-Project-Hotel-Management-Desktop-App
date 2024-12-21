using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Models.BusinessLogic;



namespace MVVMFirma.ViewModels
{
    public class NowyUdogodnieniaKlasPokojuViewModel : JedenViewModel<UdogodnieniaKlasPokoju>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyUdogodnieniaKlasPokojuViewModel()
            :base("Udogodnienie klasy pokoju")
        {
            item = new UdogodnieniaKlasPokoju();
            db = new HotelEntities();
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

        public IQueryable<KeyAndValue> KlasaPokojuItems
        {
            get
            {
                return new KlasaPokojuB(db).GetKlasaPokojuKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> UdogodnienieItems
        {
            get
            {
                return new UdogodnienieB(db).GetUdogodnienieKeyAndValueItems();
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
