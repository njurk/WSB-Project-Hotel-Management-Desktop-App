using MVVMFirma.ViewModels;
using MVVMFirma.Views;
using System.Globalization;
using System.Windows;

namespace MVVMFirma
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pl-PL");

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)
                )
            );

            MainWindow window = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            window.Show();
        }
    }

}
