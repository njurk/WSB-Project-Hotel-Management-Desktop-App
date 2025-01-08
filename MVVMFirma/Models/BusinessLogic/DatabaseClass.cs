using MVVMFirma.Models.Entities;

namespace MVVMFirma.Models.BusinessLogic
{
    public class DatabaseClass
    {
        #region Context
        public HotelEntities db { get; set; }
        #endregion

        #region Constructor
        public DatabaseClass(HotelEntities db)
        {
            this.db = db;
        }
        #endregion
    }
}
