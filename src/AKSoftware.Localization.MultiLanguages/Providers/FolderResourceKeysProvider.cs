using System;
using System.Globalization;
using System.IO;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
	public class FolderResourceKeysProvider : IKeysProvider
	{

		private readonly string _folderPath;

		public FolderResourceKeysProvider(string folderPath)
		{
			if (string.IsNullOrWhiteSpace(folderPath))
				throw new ArgumentNullException(nameof(folderPath));

			_folderPath = folderPath;
		}

		/// <summary>
		/// Retrieve all the keys based on <see	cref="CultureInfo"/> object
		/// </summary>
		/// <param name="cultureName">Culture object that represents the language you are trying to retrieve</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public Keys GetKeys(CultureInfo cultureInfo)
		{
			return GetKeys(cultureInfo.Name);
		}

		/// <summary>
		/// Retrieve all the keys based a culture name. The keys will be retrieved from the YAML file existing within the provided folder path
		/// </summary>
		/// <param name="cultureName">name of the culture in the following format "en-US", "ar-SA" ..etc</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public Keys GetKeys(string cultureName)
		{
			if (string.IsNullOrWhiteSpace(cultureName))
				throw new ArgumentNullException(nameof(cultureName));

			var filePath = Path.Combine(_folderPath, $"{cultureName}.yml");
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
	}
}
