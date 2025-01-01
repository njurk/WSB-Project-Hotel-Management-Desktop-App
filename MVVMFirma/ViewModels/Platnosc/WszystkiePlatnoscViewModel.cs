using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePlatnoscViewModel : WszystkieViewModel<PlatnoscForAllView>
    {
        #region Constructor
        public WszystkiePlatnoscViewModel()
        {
            base.DisplayName = "Płatności";
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.Platnosc.Remove(hotelEntities.Platnosc.FirstOrDefault(f => f.IdPlatnosci == SelectedItem.IdPlatnosci));
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
            List = new ObservableCollection<PlatnoscForAllView>
                (
                    from platnosc in hotelEntities.Platnosc
                    select new PlatnoscForAllView
                    {
                        IdPlatnosci = platnosc.IdPlatnosci,
                        NrPlatnosci = platnosc.NrPlatnosci,
                        IdRezerwacji = platnosc.IdRezerwacji,
                        SposobPlatnosciNazwa = platnosc.SposobPlatnosci.Nazwa,
                        StatusPlatnosciNazwa = platnosc.StatusPlatnosci.Nazwa,
                        DataPlatnosci = platnosc.DataPlatnosci,
                        Kwota = platnosc.Kwota
                    }
                );
        }
        #endregion
    }
}
