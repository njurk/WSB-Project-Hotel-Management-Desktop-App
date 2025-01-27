using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class PracownicyModalneView : ModalViewBase
    {
        public PracownicyModalneView()
        {
            InitializeComponent();
            DataContext = new PracownicyModalneViewModel();
        }
    }
}
