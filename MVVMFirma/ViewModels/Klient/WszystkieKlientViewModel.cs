using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKlientViewModel : WszystkieViewModel<Klient>
    {
        #region Constructor
        public WszystkieKlientViewModel()
            : base()
        {
            base.DisplayName = "Klienci";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<Klient>
            (
                hotelEntities.Klient.ToList()
            );
        }
        #endregion
    }
}
