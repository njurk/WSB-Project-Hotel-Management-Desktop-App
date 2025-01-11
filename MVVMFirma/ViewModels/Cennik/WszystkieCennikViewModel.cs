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
    public class WszystkieCennikViewModel : WszystkieViewModel<CennikForAllView>
    {
        #region Constructor
        public WszystkieCennikViewModel()
            : base("Cenniki")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<CennikForAllView>
                (
                    from cennik in hotelEntities.Cennik
                    select new CennikForAllView
                    {
                        IdCennika = cennik.IdCennika,
                        KlasaPokojuNazwa = cennik.KlasaPokoju.Nazwa,
                        TypPokojuNazwa = cennik.TypPokoju.Nazwa,
                        CenaDorosly = cennik.CenaDorosly,
                        CenaDziecko = cennik.CenaDziecko,
                        CenaZwierzeta = cennik.CenaZwierzeta
                    }
                );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany cennik?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Cennik.Remove(hotelEntities.Cennik.FirstOrDefault(f => f.IdCennika == SelectedItem.IdCennika));
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdCennika);
            }
        }

        // OnMessageReceived obsługuje wiadomość dotyczącą odświeżenia listy w widoku Wszystkie..View, wysłaną przy zapisie edytowanego rekordu 
        private void OnMessageReceived(string message)
        {
            if (message == "CennikRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
