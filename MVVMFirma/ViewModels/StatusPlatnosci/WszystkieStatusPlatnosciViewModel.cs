using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusPlatnosciViewModel : WszystkieViewModel<StatusPlatnosci>
    {
        #region Constructor
        public WszystkieStatusPlatnosciViewModel()
            : base()
        {
            base.DisplayName = "Statusy płatności";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<StatusPlatnosci>
            (
                hotelEntities.StatusPlatnosci.ToList()
            );
        }
        #endregion
    }
}
