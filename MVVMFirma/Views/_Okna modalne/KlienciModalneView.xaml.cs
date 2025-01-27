using MVVMFirma.ViewModels;

namespace MVVMFirma.Views
{
    public partial class KlienciModalneView : ModalViewBase
    {
        public KlienciModalneView()
        {
            InitializeComponent();
            DataContext = new KlienciModalneViewModel();
        }
    }
}
