using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKrajViewModel : WszystkieViewModel<KrajForAllView>
    {
        #region Constructor
        public WszystkieKrajViewModel()
            : base("Kraje")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<KrajForAllView>
            (
                from kraj in hotelEntities.Kraj
                select new KrajForAllView
                {
                    IdKraju = kraj.IdKraju,
                    Nazwa = kraj.Nazwa
                }
            );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany kraj:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Kraj.Remove(hotelEntities.Kraj.FirstOrDefault(f => f.IdKraju == SelectedItem.IdKraju));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdKraju);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "KrajRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
