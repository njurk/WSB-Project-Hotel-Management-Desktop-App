using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowySposobPlatnosciViewModel : JedenViewModel<SposobPlatnosci>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowySposobPlatnosciViewModel()
            : base("Sposób płatności")
        {
            db = new HotelEntities();
            item = new SposobPlatnosci();
        }

        public NowySposobPlatnosciViewModel(int itemId)
            : base("Edycja sposobu płatności")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.SposobPlatnosci.FirstOrDefault(s => s.IdSposobuPlatnosci == itemId);

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
            if (item.IdSposobuPlatnosci == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.SposobPlatnosci.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.SposobPlatnosci.FirstOrDefault(f => f.IdSposobuPlatnosci == item.IdSposobuPlatnosci);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("SposobPlatnosciRefresh");
        }
        #endregion
    }
}
