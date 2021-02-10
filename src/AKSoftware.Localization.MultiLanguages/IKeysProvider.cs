using System.Globalization;

namespace AKSoftware.Localization.MultiLanguages
{
    public interface IKeysProvider
    {
        Keys GetKeys(CultureInfo cultureInfo);
        Keys GetKeys(string cultureName);
    }
}