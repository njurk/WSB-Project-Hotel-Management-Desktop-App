using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePracownikViewModel : WszystkieViewModel<PracownikForAllView>
    {
        #region Constructor
        public WszystkiePracownikViewModel()
            :base("Pracownicy")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PracownikForAllView>
                (
                    from pracownik in hotelEntities.Pracownik
                    select new PracownikForAllView
                    {
                        IdPracownika = pracownik.IdPracownika,
                        StanowiskoNazwa = pracownik.Stanowisko.Nazwa,
                        Imie = pracownik.Imie,
                        Nazwisko = pracownik.Nazwisko,
                        Ulica = pracownik.Ulica,
                        NrDomu = pracownik.NrDomu,
                        NrLokalu = pracownik.NrLokalu,
                        KodPocztowy = pracownik.KodPocztowy,
                        Miasto = pracownik.Miasto,
                        Kraj = pracownik.Kraj.Nazwa,
                        DataUrodzenia = pracownik.DataUrodzenia,
                        Email = pracownik.Email,
                        Telefon = pracownik.Telefon
                    }
                );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybranego pracownika:\n" + SelectedItem.Imie + " " + SelectedItem.Nazwisko, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Pracownik.Remove(hotelEntities.Pracownik.FirstOrDefault(f => f.IdPracownika == SelectedItem.IdPracownika));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPracownika);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "PracownikRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
