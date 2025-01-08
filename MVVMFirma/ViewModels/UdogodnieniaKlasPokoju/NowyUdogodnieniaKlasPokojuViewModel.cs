using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;



namespace MVVMFirma.ViewModels
{
    public class NowyUdogodnieniaKlasPokojuViewModel : JedenViewModel<UdogodnieniaKlasPokoju>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyUdogodnieniaKlasPokojuViewModel()
            : base("Udogodnienie klasy pokoju")
        {
            db = new HotelEntities();
            item = new UdogodnieniaKlasPokoju();
        }

        public NowyUdogodnieniaKlasPokojuViewModel(int itemId)
            : base("Edycja udogodnienia klasy pokoju")
        {
            db = new HotelEntities();
            item = db.UdogodnieniaKlasPokoju.FirstOrDefault(u => u.IdPolaczenia == itemId);

            if (item != null)
            {
                IdKlasyPokoju = item.IdKlasyPokoju;
                IdUdogodnienia = item.IdUdogodnienia;
            }
        }

        #endregion

        #region Properties
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
            if (item.IdPolaczenia == 0) // brak ID = insert
            {
                db.UdogodnieniaKlasPokoju.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.UdogodnieniaKlasPokoju.FirstOrDefault(f => f.IdPolaczenia == item.IdPolaczenia);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("UdogodnieniaKlasPokojuRefresh");
        }
        #endregion
    }
}
