using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class ZnizkaB : DatabaseClass
    {
        #region Constructor
        public ZnizkaB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetZnizkaKeyAndValueItems()
        {
            return
                (
                    from znizka in db.Znizka
                    select new KeyAndValue
                    {
                        Key = znizka.IdZnizki,
                        Value = znizka.Nazwa + " - " + znizka.Wartosc + "%"
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
