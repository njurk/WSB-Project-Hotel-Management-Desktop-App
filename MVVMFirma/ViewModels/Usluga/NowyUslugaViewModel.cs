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
    public class NowyUslugaViewModel : JedenViewModel<Usluga>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyUslugaViewModel()
            :base("Usługa")
        {
            item = new Usluga();
            DataRozpoczeciaUslugi = DateTime.Now;
            DataZakonczeniaUslugi = DateTime.Now;
            db = new HotelEntities();
        }
        #endregion

        #region Properties
        public int IdTypuUslugi
        {
            get
            {
                return item.IdTypuUslugi;
            }
            set
            {
                item.IdTypuUslugi = value;
                OnPropertyChanged(() => IdTypuUslugi);
            }
        }

        public DateTime DataRozpoczeciaUslugi
        {
            get
            {
                return item.DataRozpoczeciaUslugi;
            }
            set
            {
                item.DataRozpoczeciaUslugi = value;
                OnPropertyChanged(() => DataRozpoczeciaUslugi);
            }
        }

        public DateTime DataZakonczeniaUslugi
        {
            get
            {
                return item.DataZakonczeniaUslugi;
            }
            set
            {
                item.DataZakonczeniaUslugi = value;
                OnPropertyChanged(() => DataZakonczeniaUslugi);
            }
        }

        public int IdKlienta
        {
            get
            {
                return item.IdKlienta;
            }
            set
            {
                item.IdKlienta = value;
                OnPropertyChanged(() => IdKlienta);
            }
        }

        public IQueryable<KeyAndValue> TypUslugiItems
        {
            get
            {
                return new TypUslugiB(db).GetTypUslugiKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> KlientItems
        {
            get
            {
                return new KlientB(db).GetKlientKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Usluga.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
