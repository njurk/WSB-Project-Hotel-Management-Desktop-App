using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
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
    public class WszystkieSposobPlatnosciViewModel : WszystkieViewModel<SposobPlatnosciForAllView>
    {
        #region Constructor
        public WszystkieSposobPlatnosciViewModel()
            : base("Sposoby płatności")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SposobPlatnosciForAllView>
            (
                from sposobplatnosci in hotelEntities.SposobPlatnosci
                select new SposobPlatnosciForAllView
                {
                    IdSposobuPlatnosci = sposobplatnosci.IdSposobuPlatnosci,
                    Nazwa = sposobplatnosci.Nazwa
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany sposób płatności:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.SposobPlatnosci.Remove(hotelEntities.SposobPlatnosci.FirstOrDefault(f => f.IdSposobuPlatnosci == SelectedItem.IdSposobuPlatnosci));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdSposobuPlatnosci);
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "SposobPlatnosciRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
