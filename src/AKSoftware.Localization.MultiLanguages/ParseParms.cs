using System.Collections.Generic;

namespace AKSoftware.Localization.MultiLanguages
{
    public class ParseParms
    {
        public List<string> Prefixes { get; set; } = new List<string>() { "Dynamic"};
        public List<string> SourceDirectories { get; set; } = new List<string>();
        public List<string> WildcardPatterns { get; set; } = new List<string>();
        public List<string> ExcludeDirectories { get; set; } = new List<string>();
        public List<string> ExcludeFiles { get; set; } = new List<string>();
        public string ResourceFilePath { get; set; }
        public string KeyReference { get; set; } = "Language";
        public string InjectLanguageContainerCode { get; set; } = @"[Inject] private ILanguageContainerService Language { get; set; }";
        public string InitLanguageContainerCode { get; set; } = @"Language.InitLocalizedComponent(this);";
        public bool RemoveLocalizedKeys { get; set; } = false;
    }
}
