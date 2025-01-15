using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class StanowiskoB : DatabaseClass
    {
        #region Constructor
        public StanowiskoB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IEnumerable<KeyAndValue> GetStanowiskoKeyAndValueItems()
        {
            return
                (
                    from stanowisko in db.Stanowisko
                    select new KeyAndValue
                    {
                        Key = stanowisko.IdStanowiska,
                        Value = stanowisko.Nazwa
                    }
                ).ToList();
        }
        #endregion
    }
}
