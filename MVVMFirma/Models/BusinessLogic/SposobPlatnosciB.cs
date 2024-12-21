using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class SposobPlatnosciB : DatabaseClass
    {
        #region Constructor
        public SposobPlatnosciB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetSposobPlatnosciKeyAndValueItems()
        {
            return
                (
                    from sposobplatnosci in db.SposobPlatnosci
                    select new KeyAndValue
                    {
                        Key = sposobplatnosci.IdSposobuPlatnosci,
                        Value = sposobplatnosci.Nazwa
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
