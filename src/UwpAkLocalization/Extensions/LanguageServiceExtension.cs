using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKSoftware.Localization.MultiLanguages;

namespace UwpAkLocalization.Extensions
{
    public static class LanguageServiceExtension
    {
        public static string GetValue(ILanguageContainerService localization, string key)
        {
            return localization[key];
        }
    }
}
