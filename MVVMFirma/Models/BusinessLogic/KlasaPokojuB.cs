using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class KlasaPokojuB : DatabaseClass
    {
        #region Constructor
        public KlasaPokojuB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetKlasaPokojuKeyAndValueItems()
        {
            return
                (
                    from klasapokoju in db.KlasaPokoju
                    select new KeyAndValue
                    {
                        Key = klasapokoju.IdKlasyPokoju,
                        Value = klasapokoju.Nazwa
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
