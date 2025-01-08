using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieStatusPokojuViewModel : WszystkieViewModel<StatusPokojuForAllView>
    {
        #region Constructor
        public WszystkieStatusPokojuViewModel()
            : base("Statusy pokojów")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<StatusPokojuForAllView>
            (
                from statuspokoju in hotelEntities.StatusPokoju
                select new StatusPokojuForAllView
                {
                    IdStatusuPokoju = statuspokoju.IdStatusuPokoju,
                    Nazwa = statuspokoju.Nazwa
                }
            );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany status pokoju:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.StatusPokoju.Remove(hotelEntities.StatusPokoju.FirstOrDefault(f => f.IdStatusuPokoju == SelectedItem.IdStatusuPokoju));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdStatusuPokoju);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "StatusPokojuRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
