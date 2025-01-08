using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieZnizkaViewModel : WszystkieViewModel<ZnizkaForAllView>
    {
        #region Constructor
        public WszystkieZnizkaViewModel()
            : base("Zniżki")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ZnizkaForAllView>
            (
                from znizka in hotelEntities.Znizka
                select new ZnizkaForAllView
                {
                    IdZnizki = znizka.IdZnizki,
                    Nazwa = znizka.Nazwa,
                    Wartosc = znizka.Wartosc
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną zniżkę:\n" + SelectedItem.Nazwa + " " + SelectedItem.Wartosc, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Znizka.Remove(hotelEntities.Znizka.FirstOrDefault(f => f.IdZnizki == SelectedItem.IdZnizki));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdZnizki);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "ZnizkaRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
