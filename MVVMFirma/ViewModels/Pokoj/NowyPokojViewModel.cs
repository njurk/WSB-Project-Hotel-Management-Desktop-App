using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyPokojViewModel : JedenViewModel<Pokoj>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyPokojViewModel()
            : base("Pokój")
        {
            db = new HotelEntities();
            item = new Pokoj();
        }

        public NowyPokojViewModel(int itemId)
            : base("Edycja pokoju")
        {
            db = new HotelEntities();
            item = db.Pokoj.FirstOrDefault(p => p.IdPokoju == itemId);

            if (item != null)
            {
                IdPietra = item.IdPietra;
                NrPokoju = item.NrPokoju;
                IdTypuPokoju = item.IdTypuPokoju;
                IdKlasyPokoju = item.IdKlasyPokoju;
                IdStatusuPokoju = item.IdStatusuPokoju;
            }
        }

        #endregion

        #region Properties
        public int IdPietra
        {
            get
            {
                return item.IdPietra;
            }
            set
            {
                item.IdPietra = value;
                OnPropertyChanged(() => IdPietra);
            }
        }

        public string NrPokoju
        {
            get
            {
                return item.NrPokoju;
            }
            set
            {
                item.NrPokoju = value;
                OnPropertyChanged(() => NrPokoju);
            }
        }

        public int IdTypuPokoju
        {
            get
            {
                return item.IdTypuPokoju;
            }
            set
            {
                item.IdTypuPokoju = value;
                OnPropertyChanged(() => IdTypuPokoju);
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

        public int IdStatusuPokoju
        {
            get
            {
                return item.IdStatusuPokoju;
            }
            set
            {
                item.IdStatusuPokoju = value;
                OnPropertyChanged(() => IdStatusuPokoju);
            }
        }

        public IQueryable<KeyAndValue> PietroItems
        {
            get
            {
                return new PietroB(db).GetPietroKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> TypPokojuItems
        {
            get
            {
                return new TypPokojuB(db).GetTypPokojuKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> KlasaPokojuItems
        {
            get
            {
                return new KlasaPokojuB(db).GetKlasaPokojuKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> StatusPokojuItems
        {
            get
            {
                return new StatusPokojuB(db).GetStatusPokojuKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdPokoju == 0) // brak ID = insert
            {
                db.Pokoj.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Pokoj.FirstOrDefault(f => f.IdPokoju == item.IdPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("PokojRefresh");
        }
        #endregion
    }
}
