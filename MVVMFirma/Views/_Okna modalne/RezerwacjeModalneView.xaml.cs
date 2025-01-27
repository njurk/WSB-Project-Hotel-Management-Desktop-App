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
    }
}
