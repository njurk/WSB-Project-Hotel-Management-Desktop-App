using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyStatusPlatnosciViewModel : JedenViewModel<StatusPlatnosci>
    {
        #region Constructor
        public NowyStatusPlatnosciViewModel()
            : base("Status płatności")
        {
            item = new StatusPlatnosci();
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
            hotelEntities.StatusPlatnosci.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
