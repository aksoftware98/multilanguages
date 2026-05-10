namespace AKSoftware.Localization.MultiLanguages.WinUI.Extensions
{
    public static class LanguageServiceExtension
    {
        public static string GetValue(ILanguageContainerService localization, string key)
        {
            return localization[key];
        }
    }
}
