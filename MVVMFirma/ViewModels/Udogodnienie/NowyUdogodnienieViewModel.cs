using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    internal class NowyUdogodnienieViewModel : JedenViewModel<Udogodnienie>
    {
        #region Constructor
        public NowyUdogodnienieViewModel()
            : base("Udogodnienie")
        {
            item = new Udogodnienie();
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get
            {
                return item.Nazwa;
            }
            set
            {
                item.Nazwa = value;
                OnPropertyChanged(() => Nazwa);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Udogodnienie.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
