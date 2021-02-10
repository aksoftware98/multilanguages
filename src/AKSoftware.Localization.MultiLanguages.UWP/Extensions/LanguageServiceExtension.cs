namespace AKSoftware.Localization.MultiLanguages.UWP.Extensions
{
    public static class LanguageServiceExtension
    {
        public static string GetValue(ILanguageContainerService localization, string key)
        {
            return localization[key];
        }
    }
}
