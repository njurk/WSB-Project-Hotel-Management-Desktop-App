using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class TypPokojuB : DatabaseClass
    {
        #region Constructor
        public TypPokojuB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetTypPokojuKeyAndValueItems()
        {
            return
                (
                    from typpokoju in db.TypPokoju
                    select new KeyAndValue
                    {
                        Key = typpokoju.IdTypuPokoju,
                        Value = typpokoju.Nazwa
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
