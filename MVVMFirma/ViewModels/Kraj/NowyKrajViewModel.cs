using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyKrajViewModel : JedenViewModel<Kraj>
    {
        #region Constructor
        public NowyKrajViewModel()
            :base("Kraj")
        {
            item = new Kraj();
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get
            {
                return item.Nazwa;
            }
            set
            {
                item.Nazwa = value;
                OnPropertyChanged(() => Nazwa);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Kraj.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
