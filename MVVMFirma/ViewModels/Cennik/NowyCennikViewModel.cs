using GalaSoft.MvvmLight.Messaging;
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
    public class NowyCennikViewModel : JedenViewModel<Cennik>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyCennikViewModel()
            : base("Cennik")
        {
            db = new HotelEntities();
            item = new Cennik();
        }

        public NowyCennikViewModel(int itemId)
            : base("Edycja cennika")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Cennik.FirstOrDefault(p => p.IdCennika == itemId);

            if (item != null)
            {
                IdKlasyPokoju = item.IdKlasyPokoju;
                IdTypuPokoju = item.IdTypuPokoju;
                CenaDorosly = item.CenaDorosly;
                CenaDziecko = item.CenaDziecko;
                CenaZwierzeta = item.CenaZwierzeta;
            }
        }
        #endregion

        #region Properties
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

        public decimal CenaDorosly
        {
            get
            {
                return item.CenaDorosly;
            }
            set
            {
                item.CenaDorosly = value;
                OnPropertyChanged(() => CenaDorosly);
            }
        }

        public decimal CenaDziecko
        {
            get
            {
                return item.CenaDziecko;
            }
            set
            {
                item.CenaDziecko = value;
                OnPropertyChanged(() => CenaDziecko);
            }
        }

        public decimal CenaZwierzeta
        {
            get
            {
                return item.CenaZwierzeta;
            }
            set
            {
                item.CenaZwierzeta = value;
                OnPropertyChanged(() => CenaZwierzeta);
            }
        }
        #endregion

        #region Items
        public IQueryable<KeyAndValue> KlasaPokojuItems
        {
            get
            {
                return new KlasaPokojuB(db).GetKlasaPokojuKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> TypPokojuItems
        {
            get
            {
                return new TypPokojuB(db).GetTypPokojuKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdCennika == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Cennik.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Cennik.FirstOrDefault(f => f.IdCennika == item.IdCennika);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("CennikRefresh");
        }
        #endregion
    }
}
