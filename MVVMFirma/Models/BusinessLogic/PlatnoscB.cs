using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class PlatnoscB : DatabaseClass
    {
        #region Constructor
        public PlatnoscB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IEnumerable<KeyAndValue> GetPlatnoscKeyAndValueItems()
        {
            return
                (
                    from platnosc in db.Platnosc
                    select new KeyAndValue
                    {
                        Key = platnosc.IdPlatnosci,
                        Value = platnosc.NrPlatnosci
                    }
                ).ToList();
        }
        #endregion
    }
}
