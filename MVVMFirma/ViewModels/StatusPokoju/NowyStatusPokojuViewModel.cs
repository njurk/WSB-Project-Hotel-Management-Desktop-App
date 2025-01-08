using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyStatusPokojuViewModel : JedenViewModel<StatusPokoju>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyStatusPokojuViewModel()
            :base("Status pokoju")
        {
            db = new HotelEntities();
            item = new StatusPokoju();
        }

        public NowyStatusPokojuViewModel(int itemId)
            :base("Edycja statusu pokoju")
        {
            db = new HotelEntities();
            item = db.StatusPokoju.FirstOrDefault(s => s.IdStatusuPokoju == itemId);

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
            if (item.IdStatusuPokoju == 0) // brak ID = insert
            {
                db.StatusPokoju.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.StatusPokoju.FirstOrDefault(f => f.IdStatusuPokoju == item.IdStatusuPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("StatusPokojuRefresh");
        }
        #endregion
    }
}
