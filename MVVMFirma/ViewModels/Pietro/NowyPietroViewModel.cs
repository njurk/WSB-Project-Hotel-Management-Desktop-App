using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;

namespace MVVMFirma.ViewModels
{
    public class NowyPietroViewModel : JedenViewModel<Pietro>
    {
        #region Constructor
        public NowyPietroViewModel()
            : base("Piętro")
        {
            item = new Pietro();
        }
        #endregion

        #region Properties
        public string NrPietra
        {
            get
            {
                return item.NrPietra;
            }
            set
            {
                item.NrPietra = value;
                OnPropertyChanged(() => NrPietra);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Pietro.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
