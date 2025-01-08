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
    public class WszystkieStatusPlatnosciViewModel : WszystkieViewModel<StatusPlatnosciForAllView>
    {
        #region Constructor
        public WszystkieStatusPlatnosciViewModel()
            : base("Statusy płatności")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<StatusPlatnosciForAllView>
            (
                from statusplatnosci in hotelEntities.StatusPlatnosci
                select new StatusPlatnosciForAllView
                {
                    IdStatusuPlatnosci = statusplatnosci.IdStatusuPlatnosci,
                    Nazwa = statusplatnosci.Nazwa
                }
            );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany status płatności:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.StatusPlatnosci.Remove(hotelEntities.StatusPlatnosci.FirstOrDefault(f => f.IdStatusuPlatnosci == SelectedItem.IdStatusuPlatnosci));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdStatusuPlatnosci);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "StatusPlatnosciRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
