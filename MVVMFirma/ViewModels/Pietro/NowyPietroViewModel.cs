using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyPietroViewModel : JedenViewModel<Pietro>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyPietroViewModel()
            : base("Piętro")
        {
            db = new HotelEntities();
            item = new Pietro();
        }

        public NowyPietroViewModel(int itemId)
            : base("Edycja piętra")
        {
            db = new HotelEntities();
            item = db.Pietro.FirstOrDefault(p => p.IdPietra == itemId);

            if (item != null)
            {
                NrPietra = item.NrPietra;
            }
        }
        #endregion

        #region Properties
        public string NrPietra
        {
            get
            {
                return item.NrPietra;
            }
            set
            {
                item.NrPietra = value;
                OnPropertyChanged(() => NrPietra);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdPietra == 0) // brak ID = insert
            {
                db.Pietro.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Pietro.FirstOrDefault(f => f.IdPietra == item.IdPietra);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("PietroRefresh");
        }
        #endregion
    }
}
