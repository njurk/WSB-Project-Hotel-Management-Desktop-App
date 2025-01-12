using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePlatnoscViewModel : WszystkieViewModel<PlatnoscForAllView>
    {
        #region Constructor
        public WszystkiePlatnoscViewModel()
            :base("Płatności")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PlatnoscForAllView>
                (
                    from platnosc in hotelEntities.Platnosc
                    select new PlatnoscForAllView
                    {
                        IdPlatnosci = platnosc.IdPlatnosci,
                        NrPlatnosci = platnosc.NrPlatnosci,
                        IdRezerwacji = platnosc.IdRezerwacji,
                        SposobPlatnosciNazwa = platnosc.SposobPlatnosci.Nazwa,
                        StatusPlatnosciNazwa = platnosc.StatusPlatnosci.Nazwa,
                        DataPlatnosci = platnosc.DataPlatnosci,
                        Kwota = platnosc.Kwota
                    }
                );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną płatność:\n" + SelectedItem.NrPlatnosci, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Platnosc.Remove(hotelEntities.Platnosc.FirstOrDefault(f => f.IdPlatnosci == SelectedItem.IdPlatnosci));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPlatnosci);
                SelectedItem = null;
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "PlatnoscRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
