using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;

namespace AKSoftware.Localization.MultiLanguages
{
    public class Keys
    {
        Dictionary<string, string> keyValues = null;

        /// <summary>
        /// Initliaze the language object for a specific calture
        /// </summary>
        /// <param name="languageContent">String content that has the YAML language</param>
        public Keys(string languageContent)
        {
            initialize(languageContent);
        }

        /// <summary>
        /// Initliaze the language file from the selected culture
        /// </summary>
        /// <param name="languageContent">String content that has the YAML language</param>
        void initialize(string languageContent)
        {
            keyValues = new Deserializer().Deserialize<Dictionary<string, string>>(languageContent).Select(k => new { Key = k.Key.ToLower(), Value = k.Value }).ToDictionary(t => t.Key, t => t.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                try
                {
                    return keyValues[key.ToLower()];
                }
                catch
                {
                    return key;
                }
            }
        }
    }
}

