using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Dywidendy.Model;
using Microsoft.Practices.Unity;

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
            var container = InitContainer();
            var w = container.Resolve<Window>();
            w.Show();


        }

        private IUnityContainer InitContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<ICurrenciesSource, NbpCurrenciesSource>();
            return container;
        }
    }
}
