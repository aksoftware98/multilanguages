using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace AKSoftware.Localization.MultiLanguages.UWP.Converters
{
    public class LocalizationConverter : MultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(item => item == null))
            {
                return null;
            }

            var localization = (Application.Current as IServiceProviderHost).ServiceProvider.GetService<ILanguageContainerService>();

            if (localization == null)
            {
                return null;
            }

            var key = (string)values[0];

            var keyValues = new Dictionary<string, object>();
            if (values.Length > 1)
            {
                foreach (var o in values.Skip(1))
                {
                    var val = (string) o;
                    var parts = val.Split(":");
                    if (parts.Length == 2)
                    {
                        keyValues[parts[0]] = parts[1];
                    }
                }
            }

            return localization[key, keyValues];
        }

        public override object[] ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

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
