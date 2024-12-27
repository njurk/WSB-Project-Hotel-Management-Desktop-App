using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace MVVMFirma.ViewModels
{
    public class WszystkieUslugaViewModel : WszystkieViewModel<UslugaForAllView>
    {
        #region Constructor
        public WszystkieUslugaViewModel()
        {
            base.DisplayName = "Usługi";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<UslugaForAllView>
                (
                    from usluga in hotelEntities.Usluga
                    select new UslugaForAllView
                    {
                        IdUslugi = usluga.IdUslugi,
                        IdRezerwacji = usluga.IdRezerwacji,
                        TypUslugiNazwa = usluga.TypUslugi.Nazwa,
                        DataRozpoczeciaUslugi = usluga.DataRozpoczeciaUslugi,
                        DataZakonczeniaUslugi = usluga.DataZakonczeniaUslugi,
                        Kwota = usluga.Kwota
                    }
                );
        }
        #endregion
    }
}
