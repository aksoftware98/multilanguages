using System.IO;
using System.Linq;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages
{
    public class EmbeddedResourceKeysProvider : KeysProvider
    {

        public EmbeddedResourceKeysProvider(Assembly assembly, string resourceFolderName = "Resources") : base(assembly,resourceFolderName)
        {

        }

        protected override string[] GetLanguageFileNames()
        {
            var languageFileNames = _resourcesAssembly.GetManifestResourceNames().Where(s =>
                s.Contains(ResourceFolderName) && (s.Contains(".yml") || s.Contains(".yaml")) && s.Contains("-")).ToArray();
            return languageFileNames;
        }

        protected override string GetFileName(string cultureName)
        {
            var fileName = _resourcesAssembly.GetManifestResourceNames().SingleOrDefault(s =>
                s.Contains(ResourceFolderName) && (s.Contains($"{cultureName}.yml") || s.Contains($"{cultureName}.yaml")));
            return fileName;
        }


        protected override Keys InternalGetKeys(string fileName)
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
