using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Dywidendy.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);
            

        }
    }
}
