using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieVATViewModel : WszystkieViewModel<VATForAllView>
    {
        #region Constructor
        public WszystkieVATViewModel()
            : base("Stawki VAT")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<VATForAllView>
            (
                from vat in hotelEntities.VAT
                select new VATForAllView
                {
                    IdVat = vat.IdVat,
                    Stawka = vat.Stawka
                }
            );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybraną stawkę VAT:\n" + SelectedItem.Stawka, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.VAT.Remove(hotelEntities.VAT.FirstOrDefault(f => f.IdVat == SelectedItem.IdVat));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdVat);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "VATRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
