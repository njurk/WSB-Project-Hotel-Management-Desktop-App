using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyVATViewModel : JedenViewModel<VAT>
    {
        #region Constructor
        public NowyVATViewModel() 
            :base("Stawka VAT")
        { 
            item = new VAT();
        }
        #endregion

        #region Properties
        public string Stawka
        {
            get
            {
                return item.Stawka;
            }
            set
            {
                item.Stawka = value;
                OnPropertyChanged(() => Stawka);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.VAT.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
