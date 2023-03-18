using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
	/// <summary>
	/// Keys provider of a YAML language files embedded within an assembly
	/// </summary>
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

		private string[] GetLanguageFileNames()
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
		public Keys GetKeys(CultureInfo cultureInfo)
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
		public Keys GetKeys(string cultureName)
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

	}
}
