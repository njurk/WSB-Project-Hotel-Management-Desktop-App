using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    internal class WszystkieUdogodnienieViewModel : WszystkieViewModel<Udogodnienie>
    {
        #region Constructor
        public WszystkieUdogodnienieViewModel()
            : base()
        {
            base.DisplayName = "Udogodnienia";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Udogodnienie>
            (
                hotelEntities.Udogodnienie.ToList()
            );
        }
        #endregion
    }
}
