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
    public class WszystkiePokojViewModel : WszystkieViewModel<PokojForAllView>
    {
        #region Constructor
        public WszystkiePokojViewModel()
            :base()
        {
            base.DisplayName = "Pokoje";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PokojForAllView>
                (
                    from pokoj in hotelEntities.Pokoj
                    select new PokojForAllView
                    {
                        IdPokoju = pokoj.IdPokoju,
                        NrPokoju = pokoj.NrPokoju,
                        TypPokojuNazwa = pokoj.TypPokoju.Nazwa,
                        KlasaPokojuNazwa = pokoj.KlasaPokoju.Nazwa,
                        StatusPokojuNazwa = pokoj.StatusPokoju.Nazwa,
                        PietroNr = pokoj.Pietro.NrPietra
                    }
                );
        }
        #endregion
    }
}
