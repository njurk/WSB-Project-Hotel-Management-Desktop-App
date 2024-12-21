using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyTypUslugiViewModel : JedenViewModel<TypUslugi>
    {
        #region Constructor
        public NowyTypUslugiViewModel()
            : base("Typ usługi")
        {
            item = new TypUslugi();
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
            hotelEntities.TypUslugi.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
