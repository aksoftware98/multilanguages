using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
	public class FolderResourceKeysProvider : BaseKeysProvider
	{

		private readonly string _folderPath;

		public FolderResourceKeysProvider(string folderPath)
		{
			if (string.IsNullOrWhiteSpace(folderPath))
				throw new ArgumentNullException(nameof(folderPath));

			if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The folder path {folderPath} does not exist");

            _folderPath = folderPath;
		}

		/// <summary>
		/// Retrieve all the keys based on <see	cref="CultureInfo"/> object
		/// </summary>
		/// <param name="cultureName">Culture object that represents the language you are trying to retrieve</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public override Keys GetKeys(CultureInfo cultureInfo)
		{
			return GetKeys(cultureInfo.Name);
		}

		/// <summary>
		/// Retrieve all the keys based a culture name. The keys will be retrieved from the YAML file existing within the provided folder path
		/// </summary>
		/// <param name="cultureName">name of the culture in the following format "en-US", "ar-SA" ..etc</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public override Keys GetKeys(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName))
				throw new ArgumentNullException(nameof(cultureName));

			string filePath = Path.Combine(_folderPath, $"{cultureName}.yml");

            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(_folderPath, $"{cultureName}.yaml");
            }

			if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file {cultureName}.yml or {cultureName}.yaml does not exist in the folder {_folderPath}");

            try
			{
				// Read the content of the file 
				var fileContent = File.ReadAllText(filePath);
				return new Keys(fileContent);
			}
			catch (Exception)
			{
				// TODO: Handle the error of the file 
				throw;
			}
		}

        /// <summary>
        /// Get a list of the registered languages
        /// </summary>
        public override List<CultureInfo> RegisteredLanguages
        {
            get
            {
				List<string> files = Directory.GetFiles(_folderPath, "*.yml").ToList();
                files.AddRange(Directory.GetFiles(_folderPath, "*.yaml"));

                List<CultureInfo> cultures = new List<CultureInfo>();

                foreach (var file in files)
                {
                    if (YamlFilePattern.IsMatch(file))
                    {
                        string cultureName = Path.GetFileNameWithoutExtension(file);
                        cultures.Add(new CultureInfo(cultureName));
                    }
                }

				return cultures;
            }

        }
    }
}
