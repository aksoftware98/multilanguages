using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages
{
    public class EmbeddedResourceKeysProvider : IKeysProvider
    {

        public EmbeddedResourceKeysProvider(Assembly assembly, string resourceFolderName)
        {
            _resourcesAssembly = assembly;
            ResourceFolderName = resourceFolderName;
        }

        public string ResourceFolderName { get; private set; }
        private readonly Assembly _resourcesAssembly;

        public Keys GetKeys(string cultureName)
        {
            var fileName = _resourcesAssembly.GetManifestResourceNames().SingleOrDefault(s => s.Contains(ResourceFolderName) && (s.Contains($"{cultureName}.yml") || s.Contains($"{cultureName}.yaml")));
            var keys = InternalGetKeys(fileName);

            if (keys == null)
            {
                throw new FileNotFoundException(
                    $"There are no language files for '{cultureName}' existing in the Resources folder within '{_resourcesAssembly.GetName().Name}' assembly.");
            }

            return keys;
        }

        public Keys GetKeys(CultureInfo culture)
        {

            var languageFileNames = _resourcesAssembly.GetManifestResourceNames().Where(s => s.Contains(ResourceFolderName) && (s.Contains(".yml") || s.Contains(".yaml")) && s.Contains("-")).ToArray();

            // Get the keys from the file that has the current culture 
            var keys = InternalGetKeys(languageFileNames.SingleOrDefault(n => n.Contains($"{culture.Name}.yml") || n.Contains($"{culture.Name}.yaml")));

            // Get the keys from a file that has the same language 
            if (keys == null)
            {
                var language = culture.Name.Split('-')[0];
                keys = InternalGetKeys( languageFileNames.FirstOrDefault(n => n.Contains(language)));
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

        private Keys InternalGetKeys(string fileName)
        {
            try
            {
                // Read the file 
                using (var fileStream = _resourcesAssembly.GetManifestResourceStream(fileName))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        return new Keys(streamReader.ReadToEnd());
                    }
                }
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
