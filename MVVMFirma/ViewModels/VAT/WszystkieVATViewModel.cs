using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieVATViewModel : WszystkieViewModel<VAT>
    {
        #region Constructor
        public WszystkieVATViewModel()
            : base()
        {
            base.DisplayName = "Stawki VAT";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<VAT>
            (
                hotelEntities.VAT.ToList()
            );
        }
        #endregion
    }
}
