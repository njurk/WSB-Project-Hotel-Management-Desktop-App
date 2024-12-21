using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePietroViewModel : WszystkieViewModel<Pietro>
    {
        #region Constructor
        public WszystkiePietroViewModel()
            : base()
        {
            base.DisplayName = "Piętra";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Pietro>
            (
                hotelEntities.Pietro.ToList()
            );
        }
        #endregion
    }
}
