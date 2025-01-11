using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyRodzajPracownikaViewModel : JedenViewModel<RodzajPracownika>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyRodzajPracownikaViewModel()
            : base("Rodzaj pracownika")
        {
            db = new HotelEntities();
            item = new RodzajPracownika();
        }

        public NowyRodzajPracownikaViewModel(int itemId)
            : base("Edycja rodzaju pracownika")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.RodzajPracownika.FirstOrDefault(r => r.IdRodzajuPracownika == itemId);

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
            if (item.IdRodzajuPracownika == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.RodzajPracownika.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.RodzajPracownika.FirstOrDefault(f => f.IdRodzajuPracownika == item.IdRodzajuPracownika);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("RodzajPracownikaRefresh");
        }
        #endregion
    }
}
