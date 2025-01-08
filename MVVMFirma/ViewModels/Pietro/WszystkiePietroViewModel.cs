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
    public class WszystkiePietroViewModel : WszystkieViewModel<PietroForAllView>
    {
        #region Constructor
        public WszystkiePietroViewModel()
            : base("Piętra")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PietroForAllView>
            (
                from pietro in hotelEntities.Pietro
                select new PietroForAllView
                {
                    IdPietra = pietro.IdPietra,
                    NrPietra = pietro.NrPietra
                }
            );
        }

        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrane piętro:\n" + SelectedItem.NrPietra, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Pietro.Remove(hotelEntities.Pietro.FirstOrDefault(f => f.IdPietra == SelectedItem.IdPietra));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPietra);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "PietroRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
