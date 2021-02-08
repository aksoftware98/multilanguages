using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AKSoftware.Localization.MultiLanguages.UWP.Converters
{
    public class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var localization = (Application.Current as IServiceProviderHost).ServiceProvider.GetService<ILanguageContainerService>();
            var key = (value as string);
            return localization[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
