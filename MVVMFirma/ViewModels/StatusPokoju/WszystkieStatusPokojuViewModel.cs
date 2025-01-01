using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusPokojuViewModel : WszystkieViewModel<StatusPokoju>
    {
        #region Constructor
        public WszystkieStatusPokojuViewModel()
            : base()
        {
            base.DisplayName = "Statusy pokojów";
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.StatusPokoju.Remove(hotelEntities.StatusPokoju.FirstOrDefault(f => f.IdStatusuPokoju == SelectedItem.IdStatusuPokoju));
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
            List = new ObservableCollection<StatusPokoju>
            (
                hotelEntities.StatusPokoju.ToList()
            );
        }
        #endregion
    }
}
