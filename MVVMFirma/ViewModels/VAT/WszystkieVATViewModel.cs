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

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.VAT.Remove(hotelEntities.VAT.FirstOrDefault(f => f.IdVat == SelectedItem.IdVat));
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
            List = new ObservableCollection<VAT>
            (
                hotelEntities.VAT.ToList()
            );
        }
        #endregion
    }
}
