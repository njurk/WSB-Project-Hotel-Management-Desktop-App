using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class RezerwacjaB : DatabaseClass
    {
        #region Constructor
        public RezerwacjaB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IEnumerable<KeyAndValue> GetRezerwacjaKeyAndValueItems()
        {
            return
                (
                    from rezerwacja in db.Rezerwacja
                    select new KeyAndValue
                    {
                        Key = rezerwacja.IdRezerwacji,
                        Value = rezerwacja.NrRezerwacji
                    }
                ).ToList();
        }
        #endregion
    }
}
