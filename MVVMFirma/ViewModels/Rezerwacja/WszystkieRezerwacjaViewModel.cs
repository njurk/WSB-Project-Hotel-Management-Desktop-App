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
            // odbieranie wiadomości odświeżenia listy
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
                    Uwagi = rezerwacja.Uwagi,
                    Znizka = rezerwacja.Znizka.Wartosc
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
                Load();
            }
        }

        // w celu edycji wybranego rekordu wysyłana jest wiadomość zawierająca jego ID
        // odbiera i obsługuje ją metoda open() w klasie MainWindowViewModel
        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdRezerwacji);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
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
