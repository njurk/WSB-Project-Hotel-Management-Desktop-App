using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class PietroB : DatabaseClass
    {
        #region Constructor
        public PietroB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetPietroKeyAndValueItems()
        {
            return
                (
                    from pietro in db.Pietro
                    select new KeyAndValue
                    {
                        Key = pietro.IdPietra,
                        Value = pietro.NrPietra
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
