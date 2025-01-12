using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class UdogodnienieB : DatabaseClass
    {
        #region Constructor
        public UdogodnienieB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IEnumerable<KeyAndValue> GetUdogodnienieKeyAndValueItems()
        {
            return
                (
                    from udogodnienie in db.Udogodnienie
                    select new KeyAndValue
                    {
                        Key = udogodnienie.IdUdogodnienia,
                        Value = udogodnienie.Nazwa
                    }
                ).ToList();
        }
        #endregion
    }
}
