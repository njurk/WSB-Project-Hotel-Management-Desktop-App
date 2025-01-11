using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieUdogodnieniaKlasPokojuViewModel : WszystkieViewModel<UdogodnieniaKlasPokojuForAllView>
    {
        #region Constructor
        public WszystkieUdogodnieniaKlasPokojuViewModel()
            :base("Udogodnienia klas pokojów")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<UdogodnieniaKlasPokojuForAllView>
                (
                    from udogodnieniaKlas in hotelEntities.UdogodnieniaKlasPokoju
                    select new UdogodnieniaKlasPokojuForAllView
                    {
                        IdPolaczenia = udogodnieniaKlas.IdPolaczenia,
                        NazwaKlasyPokoju = udogodnieniaKlas.KlasaPokoju.Nazwa,
                        NazwaUdogodnienia = udogodnieniaKlas.Udogodnienie.Nazwa
                    }
                );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrane udogodnienie klasy pokoju:\n" + SelectedItem.NazwaKlasyPokoju + " - " + SelectedItem.NazwaUdogodnienia, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.UdogodnieniaKlasPokoju.Remove(hotelEntities.UdogodnieniaKlasPokoju.FirstOrDefault(f => f.IdPolaczenia == SelectedItem.IdPolaczenia));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPolaczenia);
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "UdogodnieniaKlasPokojuRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
