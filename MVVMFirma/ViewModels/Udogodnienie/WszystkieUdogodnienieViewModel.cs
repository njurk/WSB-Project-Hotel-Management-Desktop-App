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
    internal class WszystkieUdogodnienieViewModel : WszystkieViewModel<UdogodnienieForAllView>
    {
        #region Constructor
        public WszystkieUdogodnienieViewModel()
            : base("Udogodnienia")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<UdogodnienieForAllView>
            (
                from udogodnienie in hotelEntities.Udogodnienie
                select new UdogodnienieForAllView
                {
                    IdUdogodnienia = udogodnienie.IdUdogodnienia,
                    Nazwa = udogodnienie.Nazwa
                }
            );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrane udogodnienie:\n" + SelectedItem.Nazwa, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Udogodnienie.Remove(hotelEntities.Udogodnienie.FirstOrDefault(f => f.IdUdogodnienia == SelectedItem.IdUdogodnienia));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdUdogodnienia);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "UdogodnienieRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
