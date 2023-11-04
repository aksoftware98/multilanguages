
using System.Globalization;
using AKSoftware.Localization.MultiLanguages.Providers;

namespace AKSoftware.Localization.MultiLanguages
{
    //NB: For backwards compatibility
    public class LanguageContainerInAssembly : LanguageContainer
    {
        public LanguageContainerInAssembly(CultureInfo culture, IKeysProvider keyProvider) : base(culture, keyProvider)
        {
        }

        public LanguageContainerInAssembly(IKeysProvider keysProvider) : base(keysProvider)
        {
        }
    }
}

