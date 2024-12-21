using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyRodzajPracownikaViewModel : JedenViewModel<RodzajPracownika>
    {
        #region Constructor
        public NowyRodzajPracownikaViewModel()
            : base("Rodzaj pracownika")
        {
            item = new RodzajPracownika();
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
            hotelEntities.RodzajPracownika.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
