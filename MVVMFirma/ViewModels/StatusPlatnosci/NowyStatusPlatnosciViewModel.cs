using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyStatusPlatnosciViewModel : JedenViewModel<StatusPlatnosci>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyStatusPlatnosciViewModel()
            : base("Status płatności")
        {
            db = new HotelEntities();
            item = new StatusPlatnosci();
        }

        public NowyStatusPlatnosciViewModel(int itemId)
            :base("Edycja statusu płatności")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.StatusPlatnosci.FirstOrDefault(s => s.IdStatusuPlatnosci == itemId);

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
            if (item.IdStatusuPlatnosci == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.StatusPlatnosci.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.StatusPlatnosci.FirstOrDefault(f => f.IdStatusuPlatnosci == item.IdStatusuPlatnosci);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("StatusPlatnosciRefresh");
        }
        #endregion
    }
}
