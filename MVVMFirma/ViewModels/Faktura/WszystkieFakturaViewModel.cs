using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

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
                    let sumaPlatnosci = hotelEntities.Platnosc
                        .Where(p => p.IdRezerwacji == faktura.IdRezerwacji)
                        .Sum(p => (decimal?)p.Kwota) ?? 0
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
                        Zaplacono = sumaPlatnosci,
                        Opis = faktura.Opis
                    }
                );
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.Faktura.Remove(hotelEntities.Faktura.FirstOrDefault(f => f.IdFaktury == SelectedItem.IdFaktury));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            
        }
        #endregion
    }
}
