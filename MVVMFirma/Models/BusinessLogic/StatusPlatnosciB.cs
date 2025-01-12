using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class StatusPlatnosciB : DatabaseClass
    {
        #region Constructor
        public StatusPlatnosciB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IEnumerable<KeyAndValue> GetStatusPlatnosciKeyAndValueItems()
        {
            return
                (
                    from statusplatnosci in db.StatusPlatnosci
                    select new KeyAndValue
                    {
                        Key = statusplatnosci.IdStatusuPlatnosci,
                        Value = statusplatnosci.Nazwa
                    }
                ).ToList();
        }
        #endregion
    }
}
