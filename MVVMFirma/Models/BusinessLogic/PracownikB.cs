using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models.BusinessLogic
{
    public class PracownikB : DatabaseClass
    {
        #region Constructor
        public PracownikB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IEnumerable<KeyAndValue> GetPracownikKeyAndValueItems()
        {
            return
                (
                    from pracownik in db.Pracownik
                    select new KeyAndValue
                    {
                        Key = pracownik.IdPracownika,
                        Value = pracownik.Imie + " " + pracownik.Nazwisko
                    }
                ).ToList();
        }
        #endregion
    }
}
