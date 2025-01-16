using System.Windows;

namespace MVVMFirma.Views
{
    public class ModalViewBase : Window
    {
        static ModalViewBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalViewBase), new FrameworkPropertyMetadata(typeof(ModalViewBase)));
        }
    }
}
