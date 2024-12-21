using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.BusinessLogic
{
    public class TypUslugiB : DatabaseClass
    {
        #region Constructor
        public TypUslugiB(HotelEntities db)
            : base(db) { }
        #endregion

        #region Funkcje biznesowe
        public IQueryable<KeyAndValue> GetTypUslugiKeyAndValueItems()
        {
            return
                (
                    from typuslugi in db.TypUslugi
                    select new KeyAndValue
                    {
                        Key = typuslugi.IdTypuUslugi,
                        Value = typuslugi.Nazwa
                    }
                ).ToList().AsQueryable();
        }
        #endregion
    }
}
