using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages
{
    public abstract class KeysProvider : IKeysProvider
    {
        protected KeysProvider(Assembly resourcesAssembly, string resourceFolderName = "Resources")
        {
            ResourceFolderName = resourceFolderName;
            _resourcesAssembly = resourcesAssembly;
        }

        protected readonly Assembly _resourcesAssembly;

        protected string ResourceFolderName { get; private set; }
       
        protected abstract string GetFileName(string cultureName);

        protected abstract string[] GetLanguageFileNames();

        protected abstract Keys InternalGetKeys(string fileName);

        public Keys GetKeys(CultureInfo culture)
        {

            var languageFileNames = GetLanguageFileNames();

            // Get the keys from the file that has the current culture 
            var keys = InternalGetKeys(languageFileNames.SingleOrDefault(n => n.Contains($"{culture.Name}.yml") || n.Contains($"{culture.Name}.yaml")));

            // Get the keys from a file that has the same language 
            if (keys == null)
            {
                var language = culture.Name.Split('-')[0];
                keys = InternalGetKeys(languageFileNames.FirstOrDefault(n => n.Contains(language)));
            }

            // Get the keys from the english resource 
            if (keys == null && culture.Name != "en-US")
                keys = InternalGetKeys(languageFileNames.SingleOrDefault(n => n.Contains($"en-US.yml")));

            if (keys == null)
                keys = InternalGetKeys(languageFileNames.FirstOrDefault());

            if (keys == null)
                throw new FileNotFoundException($"There are no language files existing in the Resource folder within '{_resourcesAssembly.GetName().Name}' assembly");

            return keys;
        }

      
       

        public  Keys GetKeys(string cultureName)
        {
            var fileName = GetFileName($"{cultureName}");
            var keys = InternalGetKeys(fileName);

            if (keys == null)
            {
                throw new FileNotFoundException(
                    $"There are no language files for '{cultureName}' existing in the Resources folder within '{_resourcesAssembly.GetName().Name}' assembly.");
            }

            return keys;
        }
    }
}