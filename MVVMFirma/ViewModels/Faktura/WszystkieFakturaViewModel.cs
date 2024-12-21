using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieFakturaViewModel : WszystkieViewModel<FakturaForAllView>
    {
        #region Constructor
        public WszystkieFakturaViewModel()
        {
            base.DisplayName = "Faktury";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<FakturaForAllView>
                (
                    from faktura in hotelEntities.Faktura
                    select new FakturaForAllView
                    {
                        IdFaktury = faktura.IdFaktury,
                        KlientImie = faktura.Klient.Imie,
                        KlientNazwisko = faktura.Klient.Nazwisko,
                        KlientNIP = faktura.Klient.NIP,
                        IdRezerwacji = faktura.IdRezerwacji,
                        NrFaktury = faktura.NrFaktury,
                        Opis = faktura.Opis,
                        DataWystawienia = faktura.DataWystawienia,
                        DataSprzedazy = faktura.DataSprzedazy,
                        KwotaNetto = faktura.KwotaNetto,
                        VAT = faktura.VAT,
                        KwotaBrutto = faktura.KwotaBrutto,
                        IdPlatnosci = faktura.IdPlatnosci,
                        TerminPlatnosci = faktura.TerminPlatnosci
                    }
                );
        }
        #endregion
    }
}
