using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
    public class EmbeddedResourceKeysProvider : IKeysProvider
    {

        private readonly Assembly _assembly;
        private readonly string _resourcesFolderName = "Resources"; 
		
        public EmbeddedResourceKeysProvider(Assembly assembly, 
                                            params string[] resourcesFolders)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (resourcesFolders == null || !resourcesFolders.Any())
                throw new ArgumentNullException(nameof(resourcesFolders)); 
			
            _assembly = assembly;
            _resourcesFolderName = string.Join(".", resourcesFolders);
        }

        public EmbeddedResourceKeysProvider(Assembly assembly,
                                            string resourcesFolderName = "Resources")
        {
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));
			if (string.IsNullOrWhiteSpace(resourcesFolderName))
				throw new ArgumentNullException(nameof(resourcesFolderName));

			_assembly = assembly;
			_resourcesFolderName = resourcesFolderName;
		}

        protected string[] GetLanguageFileNames()
        {
            var languageFileNames = _assembly
                                    .GetManifestResourceNames()
                                    .Where(s =>
                                            s.Contains(_resourcesFolderName) && 
                                                (s.Contains(".yml") || 
                                                s.Contains(".yaml")) && 
                                                s.Contains("-"))
                                    .ToArray();
            return languageFileNames;
        }

        protected string GetFileName(string cultureName)
        {
            var fileName = _assembly
                                .GetManifestResourceNames()
                                .SingleOrDefault(s =>
                                                        s.Contains(_resourcesFolderName) &&
                                                        (s.Contains($"{cultureName}.yml") || 
                                                         s.Contains($"{cultureName}.yaml")));
            return fileName;
        }

		
		public Keys GetKeys(CultureInfo cultureInfo)
		{
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));
			var cultureName = cultureInfo.Name;
			return GetKeys(cultureName);
		}

		public Keys GetKeys(string cultureName)
        {
            try
            {
                var resourcesFileName = GetFileName(cultureName); 
                // Read the file 
                using (var fileStream = _assembly.GetManifestResourceStream(resourcesFileName))
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
