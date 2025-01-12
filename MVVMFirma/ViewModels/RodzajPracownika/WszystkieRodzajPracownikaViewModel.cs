using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRodzajPracownikaViewModel : WszystkieViewModel<RodzajPracownikaForAllView>
    {
        #region Constructor
        public WszystkieRodzajPracownikaViewModel()
            : base("Rodzaje pracowników")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<RodzajPracownikaForAllView>
            (
                from rodzajpracownika in hotelEntities.RodzajPracownika
                select new RodzajPracownikaForAllView
                {
                    IdRodzajuPracownika = rodzajpracownika.IdRodzajuPracownika,
                    Nazwa = rodzajpracownika.Nazwa
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany rodzaj pracownika:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.RodzajPracownika.Remove(hotelEntities.RodzajPracownika.FirstOrDefault(f => f.IdRodzajuPracownika == SelectedItem.IdRodzajuPracownika));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdRodzajuPracownika);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "RodzajPracownikaRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
