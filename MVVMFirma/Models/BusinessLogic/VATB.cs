using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class VATB : DatabaseClass
    {
        #region Constructor
        public VATB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetVATKeyAndValueItems()
        {
            return
                (
                    from vat in db.VAT
                    select new KeyAndValue
                    {
                        Key = vat.IdVat,
                        Value = vat.Stawka
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
