using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyTypPokojuViewModel : JedenViewModel<TypPokoju>
    {
        #region Constructor
        public NowyTypPokojuViewModel()
            : base("Typ pokoju")
        {
            item = new TypPokoju();
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

        public decimal Cena
        {
            get
            {
                return item.Cena;
            }
            set
            {
                item.Cena = value;
                OnPropertyChanged(() => Cena);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.TypPokoju.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
