using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRezerwacjaViewModel : WszystkieViewModel<RezerwacjaForAllView>
    {
        #region Constructor
        public WszystkieRezerwacjaViewModel()
            :base("Rezerwacje")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<RezerwacjaForAllView>
            (
                from rezerwacja in hotelEntities.Rezerwacja
                let sumaPlatnosci = hotelEntities.Platnosc
                    .Where(p => p.IdRezerwacji == rezerwacja.IdRezerwacji)
                    .Sum(p => (decimal?)p.Kwota) ?? 0.00m
                select new RezerwacjaForAllView
                {
                    IdRezerwacji = rezerwacja.IdRezerwacji,
                    NrRezerwacji = rezerwacja.NrRezerwacji,
                    KlientImie = rezerwacja.Klient.Imie,
                    KlientNazwisko = rezerwacja.Klient.Nazwisko,
                    NrPokoju = rezerwacja.Pokoj.NrPokoju,
                    LiczbaDoroslych = rezerwacja.LiczbaDoroslych,
                    LiczbaDzieci = rezerwacja.LiczbaDzieci,
                    CzyZwierzeta = rezerwacja.CzyZwierzeta,
                    DataRezerwacji = rezerwacja.DataRezerwacji,
                    DataZameldowania = rezerwacja.DataZameldowania,
                    DataWymeldowania = rezerwacja.DataWymeldowania,
                    Kwota = rezerwacja.Kwota,
                    CzyZaplacona = sumaPlatnosci >= rezerwacja.Kwota,
                    Uwagi = rezerwacja.Uwagi
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną rezerwację:\n" + SelectedItem.NrRezerwacji, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Rezerwacja.Remove(hotelEntities.Rezerwacja.FirstOrDefault(f => f.IdRezerwacji == SelectedItem.IdRezerwacji));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdRezerwacji);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "RezerwacjaRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
