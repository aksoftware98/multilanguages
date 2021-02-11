using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using AKSoftware.Localization.MultiLanguages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using AKSoftware.Localization.MultiLanguages.UWP;
using Microsoft.Extensions.DependencyInjection;

namespace UwpAkLocalization
{
    public sealed partial class MainPage : Page
    {
        public ILanguageContainerService Localization { get; private set; }
        public MainPage()
        {
            this.InitializeComponent();
            SuppressPageAnimation();
            Localization = ((IServiceProviderHost)Application.Current).ServiceProvider.GetService<ILanguageContainerService>();
        }

        private void SuppressPageAnimation()
        {
            var transitionCollection = new TransitionCollection();
            var theme = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new SuppressNavigationTransitionInfo()
            };

            transitionCollection.Add(theme);
            Transitions = transitionCollection;
        }

        private void SetLanguage(object sender, RoutedEventArgs e)
        {
            var language = ((Button) sender).Tag.ToString();
            var culture = CultureInfo.GetCultureInfo(language);
            Localization.SetLanguage(culture);
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
