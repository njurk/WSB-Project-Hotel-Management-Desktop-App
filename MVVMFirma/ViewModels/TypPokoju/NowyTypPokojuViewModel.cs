using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyTypPokojuViewModel : JedenViewModel<TypPokoju>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyTypPokojuViewModel()
            : base("Typ pokoju")
        {
            db = new HotelEntities();
            item = new TypPokoju();
        }

        public NowyTypPokojuViewModel(int itemId)
            : base("Edycja typu pokoju")
        {
            db = new HotelEntities();
            item = db.TypPokoju.FirstOrDefault(s => s.IdTypuPokoju == itemId);

            if (item != null)
            {
                Nazwa = item.Nazwa;
                MaxLiczbaOsob = item.MaxLiczbaOsob;
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

        public string MaxLiczbaOsob
        {
            get
            {
                return item.MaxLiczbaOsob;
            }
            set
            {
                item.MaxLiczbaOsob = value;
                OnPropertyChanged(() => MaxLiczbaOsob);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdTypuPokoju == 0) // brak ID = insert
            {
                db.TypPokoju.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.TypPokoju.FirstOrDefault(f => f.IdTypuPokoju == item.IdTypuPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("TypPokojuRefresh");
        }
        #endregion
    }
}
