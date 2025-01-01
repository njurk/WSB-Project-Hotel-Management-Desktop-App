using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypPokojuViewModel : WszystkieViewModel<TypPokoju>
    {
        #region Constructor
        public WszystkieTypPokojuViewModel()
            : base()
        {
            base.DisplayName = "Typy pokojów";
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.TypPokoju.Remove(hotelEntities.TypPokoju.FirstOrDefault(f => f.IdTypuPokoju == SelectedItem.IdTypuPokoju));
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
            List = new ObservableCollection<TypPokoju>
            (
                hotelEntities.TypPokoju.ToList()
            );
        }
        #endregion
    }
}
