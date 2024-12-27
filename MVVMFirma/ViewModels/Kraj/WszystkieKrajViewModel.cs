using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKrajViewModel : WszystkieViewModel<Kraj>
    {
        #region Constructor
        public WszystkieKrajViewModel()
            : base()
        {
            base.DisplayName = "Kraje";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Kraj>
            (
                hotelEntities.Kraj.ToList()
            );
        }
        #endregion
    }
}
