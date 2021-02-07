using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using AKSoftware.Localization.MultiLanguages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpAkLocalization
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ILanguageContainerService Localization { get; private set; }
        public MainPage()
        {
            this.InitializeComponent();

            Localization = (Application.Current as App).ServiceProvider.GetService<ILanguageContainerService>();

           
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var culture = CultureInfo.GetCultureInfo("fr-FR");
            Localization.SetLanguage(culture);
            this.Frame.Navigate(typeof(MainPage));
        }

        private async Task SetLocalizationLanguage(string language, int delay = 100, bool forceNavigation = true)
        {
            // Only set if we have to...
            if (ApplicationLanguages.PrimaryLanguageOverride == language)
            {
                return;
            }

            var culture = CultureInfo.GetCultureInfo("fr-FR");
            Localization.SetLanguage(culture);

            ApplicationLanguages.PrimaryLanguageOverride = language;
            ResourceContext.GetForCurrentView().Reset();
            ResourceContext.GetForViewIndependentUse().Reset();
            await Task.Delay(delay);

            //if (forceNavigation)
            //{
            //    navigationService_.NavigateToViewModel<ShellViewModel>(new AppState { SelectedLanguage = SelectedLanguageOption, SelectedPricingOption = SelectedPricingOption });
            //}
        }
    }
}
