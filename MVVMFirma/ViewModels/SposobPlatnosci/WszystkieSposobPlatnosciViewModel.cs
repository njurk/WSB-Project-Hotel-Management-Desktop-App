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
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdSposobuPlatnosci);
            }
        }
        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
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
