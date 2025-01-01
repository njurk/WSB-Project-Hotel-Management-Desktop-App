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

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.Udogodnienie.Remove(hotelEntities.Udogodnienie.FirstOrDefault(f => f.IdUdogodnienia == SelectedItem.IdUdogodnienia));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            throw new NotImplementedException();
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
