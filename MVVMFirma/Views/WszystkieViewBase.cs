using System.Windows;
using System.Windows.Controls;

namespace MVVMFirma.Views
{
    public class WszystkieViewBase : UserControl
    {
        static WszystkieViewBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WszystkieViewBase), new FrameworkPropertyMetadata(typeof(WszystkieViewBase)));
        }
    }
}
