using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieFakturaViewModel : WszystkieViewModel<FakturaForAllView>
    {
        #region Constructor
        public WszystkieFakturaViewModel()
            :base("Faktury")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
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
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną fakturę:\n" + SelectedItem.NrFaktury, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Faktura.Remove(hotelEntities.Faktura.FirstOrDefault(f => f.IdFaktury == SelectedItem.IdFaktury));
                hotelEntities.SaveChanges();
                Load();
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdFaktury);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "FakturaRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
