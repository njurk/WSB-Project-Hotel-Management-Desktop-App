using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class StatusPokojuB : DatabaseClass
    {
        #region Constructor
        public StatusPokojuB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetStatusPokojuKeyAndValueItems()
        {
            return
                (
                    from statuspokoju in db.StatusPokoju
                    select new KeyAndValue
                    {
                        Key = statuspokoju.IdStatusuPokoju,
                        Value = statuspokoju.Nazwa
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
