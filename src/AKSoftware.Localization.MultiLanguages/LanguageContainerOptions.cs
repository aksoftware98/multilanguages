using System.Globalization;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages
{
    public class LanguageContainerOptions
    {
        /// <summary>
        /// Name of the folder that has the language resources 
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Assembly that contains the folder that has the language resources 
        /// </summary>
        public Assembly ResourcesAssembly { get; set; }

        /// <summary>
        /// Culture of the user 
        /// </summary>
        public CultureInfo UserCulture { get; set; }
    }
}

