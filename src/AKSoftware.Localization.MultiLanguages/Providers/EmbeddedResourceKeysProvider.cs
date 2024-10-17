using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
	/// <summary>
	/// Keys provider of a YAML language files embedded within an assembly
	/// </summary>
	public class EmbeddedResourceKeysProvider : BaseKeysProvider
	{
		private List<CultureInfo> _registeredLanguages;
        private readonly Assembly _assembly;
		private readonly string _resourcesFolderName = "Resources";

        /// <summary>
        /// Specify the assembly that contains the resources and the folders that contain the language files
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourcesFolders"></param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Specify the assembly that contains the resources and the folder that contains the language files
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourcesFolderName"></param>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Specify the assembly name that contains the resources and the folder that contains the language files
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="resourcesFolderName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public EmbeddedResourceKeysProvider(string assemblyName, string resourcesFolderName = "Resources")
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName));
            if (string.IsNullOrWhiteSpace(resourcesFolderName))
                throw new ArgumentNullException(nameof(resourcesFolderName));

            _assembly = FindLoadedAssembly(assemblyName);

			if (_assembly == null)
                throw new ArgumentException($"Assembly {assemblyName} not found");

            _resourcesFolderName = resourcesFolderName;
        }

        /// <summary>
        /// Specify the assembly name that contains the resources and the folders that contain the language files
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="resourcesFolders"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public EmbeddedResourceKeysProvider(string assemblyName, params string[] resourcesFolders)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName));
            if (resourcesFolders == null || !resourcesFolders.Any())
                throw new ArgumentNullException(nameof(resourcesFolders));

            _assembly = FindLoadedAssembly(assemblyName);

            if (_assembly == null)
                _assembly = LoadAssemblyByName(assemblyName);

            if (_assembly == null)
                throw new ArgumentException($"Assembly {assemblyName} not found");

            _resourcesFolderName = string.Join(".", resourcesFolders);
        }

        private Assembly FindLoadedAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        private Assembly LoadAssemblyByName(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve the full file path with an assembly resource using the file name only 
        /// </summary>
        /// <param name="cultureName">Culture name "en-US", "ar-SA" ..etc.</param>
        /// <returns><see cref="string"/> represents the full path within the assembly resource</returns>
        private string GetFilePath(string cultureName)
		{
			var fileName = _assembly
								.GetManifestResourceNames()
								.SingleOrDefault(s =>
														s.Contains(_resourcesFolderName) &&
														(s.Contains($"{cultureName}.yml") ||
														 s.Contains($"{cultureName}.yaml")));
			return fileName;
		}

		/// <summary>
		/// Retrieve <see cref="Keys"/> instance from a give <see cref="CultureInfo"/> object
		/// </summary>
		/// <param name="cultureInfo">Culture info of the requested language</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public override Keys GetKeys(CultureInfo cultureInfo)
		{
			if (cultureInfo == null)
				throw new ArgumentNullException(nameof(cultureInfo));
			var cultureName = cultureInfo.Name;
			return GetKeys(cultureName);
		}

		/// <summary>
		/// Retrieve <see cref="Keys"/> instance from a give culture name
		/// </summary>
		/// <remarks>
		/// Culture name must be in the following format "en-US", "ar-SA"..
		/// </remarks>
		/// <param name="cultureName">Name of the culture "en-US", "ar-SA" ..etc</param>
		/// <returns><see cref="Keys"/> instance</returns>
		public override Keys GetKeys(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName))
				throw new ArgumentNullException(nameof(cultureName));

			try
			{
				var resourcesFileName = GetFilePath(cultureName);
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

        /// <summary>
        /// Get a list of the registered languages
        /// </summary>
        public override IEnumerable<CultureInfo> RegisteredLanguages
        {
            get
            {
				//Lazy loading
                if (_registeredLanguages != null)
                    return _registeredLanguages;

                var languageFileNames = GetLanguageFileNames();

				var cultures = new List<CultureInfo>();
                foreach (var languageFileName in languageFileNames)
                {
					int resourceIndex = languageFileName.IndexOf(_resourcesFolderName, StringComparison.Ordinal);

					if (resourceIndex == -1)
                        continue;

                    string file = languageFileName.Substring(resourceIndex + _resourcesFolderName.Length + 1);

                    if (YamlFilePattern.IsMatch(file))
                    {
                        string cultureName = Path.GetFileNameWithoutExtension(file);
                        cultures.Add(new CultureInfo(cultureName));
                    }
                }

				_registeredLanguages = cultures;
                return _registeredLanguages;
            }
        }

        private string[] GetLanguageFileNames()
        {
            var languageFileNames = _assembly
                .GetManifestResourceNames()
                .Where(s =>
                    s.Contains(_resourcesFolderName) &&
                    (s.Contains(".yml") ||
                     s.Contains(".yaml")))
                .ToArray();
            return languageFileNames;
        }
    }
}
