using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyPokojViewModel : JedenViewModel<Pokoj>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyPokojViewModel()
            :base("Pokój")
        {
            item = new Pokoj();
            db = new HotelEntities();
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
            hotelEntities.Pokoj.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
