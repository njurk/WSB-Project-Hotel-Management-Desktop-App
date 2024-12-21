using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKlasaPokojuViewModel : WszystkieViewModel<KlasaPokoju>
    {
        #region Constructor
        public WszystkieKlasaPokojuViewModel()
            : base()
        {
            base.DisplayName = "Klasy pokojów";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<KlasaPokoju>
            (
                hotelEntities.KlasaPokoju.ToList()
            );
        }
        #endregion
    }
}
