using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieUdogodnieniaKlasPokojuViewModel : WszystkieViewModel<UdogodnieniaKlasPokojuForAllView>
    {
        #region Constructor
        public WszystkieUdogodnieniaKlasPokojuViewModel()
        {
            base.DisplayName = "Udogodnienia klas pokojów";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<UdogodnieniaKlasPokojuForAllView>
                (
                    from udogodnieniaKlas in hotelEntities.UdogodnieniaKlasPokoju
                    select new UdogodnieniaKlasPokojuForAllView
                    {
                        IdPolaczenia = udogodnieniaKlas.IdPolaczenia,
                        NazwaKlasyPokoju = udogodnieniaKlas.KlasaPokoju.Nazwa,
                        NazwaUdogodnienia = udogodnieniaKlas.Udogodnienie.Nazwa
                    }
                );
        }
        #endregion
    }
}
