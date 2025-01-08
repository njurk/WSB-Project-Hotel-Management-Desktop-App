using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypPokojuViewModel : WszystkieViewModel<TypPokojuForAllView>
    {
        #region Constructor
        public WszystkieTypPokojuViewModel()
            : base("Typy pokojów")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<TypPokojuForAllView>
            (
                from typpokoju in hotelEntities.TypPokoju
                select new TypPokojuForAllView
                {
                    IdTypuPokoju = typpokoju.IdTypuPokoju,
                    Nazwa = typpokoju.Nazwa,
                    Cena = typpokoju.Cena
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany typ pokoju:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.TypPokoju.Remove(hotelEntities.TypPokoju.FirstOrDefault(f => f.IdTypuPokoju == SelectedItem.IdTypuPokoju));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdTypuPokoju);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "TypPokojuRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
