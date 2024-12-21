using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class PokojB : DatabaseClass
    {
        #region Constructor
        public PokojB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetPokojKeyAndValueItems()
        {
            return
                (
                    from pokoj in db.Pokoj
                    select new KeyAndValue
                    {
                        Key = pokoj.IdPokoju,
                        Value = pokoj.NrPokoju
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
