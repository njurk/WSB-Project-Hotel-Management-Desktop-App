using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieSposobPlatnosciViewModel : WszystkieViewModel<SposobPlatnosci>
    {
        #region Constructor
        public WszystkieSposobPlatnosciViewModel()
            : base()
        {
            base.DisplayName = "Sposoby płatności";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SposobPlatnosci>
            (
                hotelEntities.SposobPlatnosci.ToList()
            );
        }
        #endregion
    }
}
