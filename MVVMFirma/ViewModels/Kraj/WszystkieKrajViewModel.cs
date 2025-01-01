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
        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.Kraj.Remove(hotelEntities.Kraj.FirstOrDefault(f => f.IdKraju == SelectedItem.IdKraju));
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
            List = new ObservableCollection<Kraj>
            (
                hotelEntities.Kraj.ToList()
            );
        }
        #endregion
    }
}
