using System;
using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace AKSoftware.Localization.MultiLanguages.WinUI.Converters
{
    public abstract class MultiValueConverterBase : DependencyObject, IValueConverter
    {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            var cultureInfo = !string.IsNullOrEmpty(language) ? new CultureInfo(language) : null;
            return Convert((object[])value, targetType, parameter, cultureInfo);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var cultureInfo = !string.IsNullOrEmpty(language) ? new CultureInfo(language) : null;
            return ConvertBack(value, targetType, parameter, cultureInfo);
        }
    }
}
