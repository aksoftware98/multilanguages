using System.Globalization;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
    /// <summary>
    /// Keys provider interface, it wraps the functionality of the get the keys, 
    /// </summary>
    public interface IKeysProvider
    {
		/// <summary>
        /// Retrieve an instance of <see cref="Keys"/> from a <see cref="CultureInfo"/> object
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        Keys GetKeys(CultureInfo cultureInfo);

		/// <summary>
		/// Retrieves an instance of <see cref="Keys"/> from the name of the culture, the name of the culture should be in the format of "en-US"
		/// </summary>
		/// <param name="cultureName"></param>
		/// <returns></returns>
		Keys GetKeys(string cultureName);
    }
}