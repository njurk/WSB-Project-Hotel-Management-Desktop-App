using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.ViewModels;
using System.Windows;

namespace MVVMFirma.Views
{
    public partial class KlienciModalneView : ModalViewBase
    {
        public KlienciModalneView()
        {
            InitializeComponent();
            DataContext = new KlienciModalneViewModel();
        }

        private void btnSendItem_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is KlienciModalneViewModel viewModel)
            {
                if (viewModel.SelectedItem != null)
                {
                    var idKlienta = viewModel.SelectedItem.IdKlienta;
                    Messenger.Default.Send(idKlienta);

                    Close();
                }
                else
                {
                    MessageBox.Show("Nie wybrano klienta", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
