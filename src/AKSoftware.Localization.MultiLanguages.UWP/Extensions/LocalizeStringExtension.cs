using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Markup;

namespace AKSoftware.Localization.MultiLanguages.UWP.Extensions
{
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public class LocalizeStringExtension : MarkupExtension
    {
        public string Key { get; set; }

        public bool Capitalize { get; set; }

        protected override object ProvideValue()
        {
            var language = ((IServiceProviderHost)Windows.UI.Xaml.Application.Current).ServiceProvider.GetService<ILanguageContainerService>();
            var value = language?[Key];

            return Capitalize ? value?.ToUpperInvariant() : value;

        }
    }
}
