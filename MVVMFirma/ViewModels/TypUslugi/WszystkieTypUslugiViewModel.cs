using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypUslugiViewModel : WszystkieViewModel<TypUslugi>
    {
        #region Constructor
        public WszystkieTypUslugiViewModel()
            : base()
        {
            base.DisplayName = "Typy usług";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<TypUslugi>
            (
                hotelEntities.TypUslugi.ToList()
            );
        }
        #endregion
    }
}
