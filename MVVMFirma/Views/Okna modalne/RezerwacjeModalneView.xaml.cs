using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.ViewModels;
using System.Windows;

namespace MVVMFirma.Views
{
    public partial class RezerwacjeModalneView : ModalViewBase
    {
        public RezerwacjeModalneView()
        {
            InitializeComponent();
            DataContext = new RezerwacjeModalneViewModel();
        }

        private void btnSendItem_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is RezerwacjeModalneViewModel viewModel)
            {
                if (viewModel.SelectedItem != null)
                {
                    var idRezerwacji = viewModel.SelectedItem.IdRezerwacji;
                    Messenger.Default.Send(idRezerwacji);

                    Close();
                }
                else
                {
                    MessageBox.Show("Nie wybrano rezerwacji", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
