using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class KlientB : DatabaseClass
    {
        #region Constructor
        public KlientB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetKlientKeyAndValueItems()
        {
            return
                (
                    from klient in db.Klient
                    select new KeyAndValue
                    {
                        Key = klient.IdKlienta,
                        Value = klient.Imie + " " + klient.Nazwisko
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
