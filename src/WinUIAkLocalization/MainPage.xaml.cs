using System.Globalization;
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUIAkLocalization
{
    public sealed partial class MainPage : Page
    {
        public ILanguageContainerService Localization { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            Localization = ((IServiceProviderHost)Application.Current).ServiceProvider.GetService<ILanguageContainerService>();
        }

        private void SetLanguage(object sender, RoutedEventArgs e)
        {
            var language = ((Button)sender).Tag.ToString();
            var culture = CultureInfo.GetCultureInfo(language);
            Localization.SetLanguage(culture);
        }
    }
}
