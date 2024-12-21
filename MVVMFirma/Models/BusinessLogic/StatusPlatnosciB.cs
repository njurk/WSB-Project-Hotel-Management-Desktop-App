using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class StatusPlatnosciB : DatabaseClass
    {
        #region Constructor
        public StatusPlatnosciB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetStatusPlatnosciKeyAndValueItems()
        {
            return
                (
                    from statusplatnosci in db.StatusPlatnosci
                    select new KeyAndValue
                    {
                        Key = statusplatnosci.IdStatusuPlatnosci,
                        Value = statusplatnosci.Nazwa
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
