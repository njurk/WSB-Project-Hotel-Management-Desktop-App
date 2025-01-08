using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    internal class NowyUdogodnienieViewModel : JedenViewModel<Udogodnienie>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyUdogodnienieViewModel()
            : base("Udogodnienie")
        {
            db = new HotelEntities();
            item = new Udogodnienie();
        }

        public NowyUdogodnienieViewModel(int itemId)
            : base("Edycja udogodnienia")
        {
            db = new HotelEntities();
            item = db.Udogodnienie.FirstOrDefault(u => u.IdUdogodnienia == itemId);

            if (item != null)
            {
                Nazwa = item.Nazwa;
            }
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
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdUdogodnienia == 0) // brak ID = insert
            {
                db.Udogodnienie.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Udogodnienie.FirstOrDefault(f => f.IdUdogodnienia == item.IdUdogodnienia);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("UdogodnienieRefresh");
        }
        #endregion
    }
}
