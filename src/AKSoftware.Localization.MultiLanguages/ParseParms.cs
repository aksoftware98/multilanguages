using System.Collections.Generic;

namespace AKSoftware.Localization.MultiLanguages
{
    public class ParseParms
    {
        public List<string> Prefixes { get; set; } = new List<string>() {"Required","MaxLength", "Dynamic"};
        public List<string> SourceDirectories { get; set; } = new List<string>();
        public List<string> WildcardPatterns { get; set; } = new List<string>();
        public List<string> ExcludeDirectories { get; set; } = new List<string>();
        public List<string> ExcludeFiles { get; set; } = new List<string>();
        public string ResourceFilePath { get; set; }
        public string KeyReference { get; set; } = "_lc";
        public string InjectLanguageContainerCode { get; set; } = @"[Inject] private ILanguageContainerService _lc { get; set; }";
        public string InitLanguageContainerCode { get; set; } = @"_lc.InitLocalizedComponent(this);";
        public bool RemoveLocalizedKeys { get; set; } = false;
    }
}
