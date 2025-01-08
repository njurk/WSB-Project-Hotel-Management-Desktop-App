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
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdRodzajuPracownika);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
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
