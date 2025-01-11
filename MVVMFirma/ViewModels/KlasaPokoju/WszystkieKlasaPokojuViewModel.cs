using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKlasaPokojuViewModel : WszystkieViewModel<KlasaPokojuForAllView>
    {
        #region Constructor
        public WszystkieKlasaPokojuViewModel()
            : base("Klasy pokojów")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<KlasaPokojuForAllView>
            (
                from klasapokoju in hotelEntities.KlasaPokoju
                select new KlasaPokojuForAllView
                {
                    IdKlasyPokoju = klasapokoju.IdKlasyPokoju,
                    Nazwa = klasapokoju.Nazwa
                }
            );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną klasę pokoju:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.KlasaPokoju.Remove(hotelEntities.KlasaPokoju.FirstOrDefault(f => f.IdKlasyPokoju == SelectedItem.IdKlasyPokoju));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdKlasyPokoju);
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "KlasaPokojuRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
