using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyKlasaPokojuViewModel : JedenViewModel<KlasaPokoju>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyKlasaPokojuViewModel()
            : base("Klasa pokoju")
        {
            db = new HotelEntities();
            item = new KlasaPokoju();
        }

        public NowyKlasaPokojuViewModel(int itemId)
            : base("Edycja klasy pokoju")
        {
            db = new HotelEntities();
            item = db.KlasaPokoju.FirstOrDefault(k => k.IdKlasyPokoju == itemId);

            if (item != null)
            {
                Nazwa = item.Nazwa;
                Doplata = item.Doplata;
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
            if (item.IdKlasyPokoju == 0) // brak ID = insert
            {
                db.KlasaPokoju.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.KlasaPokoju.FirstOrDefault(f => f.IdKlasyPokoju == item.IdKlasyPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }
            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("KlasaPokojuRefresh");
        }
        #endregion
    }
}
