using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStanowiskoViewModel : WszystkieViewModel<StanowiskoForAllView>
    {
        #region Constructor
        public WszystkieStanowiskoViewModel()
            : base("Stanowiska")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<StanowiskoForAllView>
            (
                from stanowisko in hotelEntities.Stanowisko
                select new StanowiskoForAllView
                {
                    IdStanowiska = stanowisko.IdStanowiska,
                    Nazwa = stanowisko.Nazwa,
                    StawkaGodzinowa = stanowisko.StawkaGodzinowa
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrane stanowisko:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Stanowisko.Remove(hotelEntities.Stanowisko.FirstOrDefault(f => f.IdStanowiska == SelectedItem.IdStanowiska));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdStanowiska);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "StanowiskoRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
