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
                        NrFaktury = faktura.NrFaktury,
                        IdRezerwacji = faktura.IdRezerwacji,
                        KlientNIP = faktura.Rezerwacja.Klient.NIP,
                        KlientImie = faktura.Rezerwacja.Klient.Imie,
                        KlientNazwisko = faktura.Rezerwacja.Klient.Nazwisko,
                        DataWystawienia = faktura.DataWystawienia,
                        DataSprzedazy = faktura.DataSprzedazy,
                        KwotaNetto = faktura.KwotaNetto,
                        VAT = faktura.VAT.Stawka,
                        KwotaBrutto = faktura.KwotaBrutto,
                        TerminPlatnosci = faktura.TerminPlatnosci,
                        IdPlatnosci = faktura.IdPlatnosci,
                        Opis = faktura.Opis
                    }
                );
        }
        #endregion
    }
}
