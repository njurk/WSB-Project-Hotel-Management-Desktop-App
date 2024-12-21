using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowySposobPlatnosciViewModel : JedenViewModel<SposobPlatnosci>
    {
        #region Constructor
        public NowySposobPlatnosciViewModel()
            : base("Sposób płatności")
        {
            item = new SposobPlatnosci();
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
            hotelEntities.SposobPlatnosci.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
