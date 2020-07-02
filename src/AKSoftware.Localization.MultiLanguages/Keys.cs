using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        JObject keyValues = null;

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
            var dynamicResult = new Deserializer().Deserialize<dynamic>(languageContent);
            string json = JsonConvert.SerializeObject(dynamicResult);
            keyValues = JObject.Parse(json);
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
                    if (key.Contains(":"))
                    {
                        string[] nestedKey = key.Split(':');
                        JObject nestedValue = (JObject)keyValues[nestedKey[0]];
                        string value = string.Empty; 
                        for (int i = 1; i < nestedKey.Length; i++)
                        {
                            if(i == nestedKey.Length - 1)
                            {
                                var result = nestedValue[nestedKey[i]];
                                if (result == null)
                                    return nestedKey[nestedKey.Length - 1];

                                return (string)result; 
                            }

                            nestedValue = (JObject)nestedValue[nestedKey[i]];
                        }

                        return value;
                    }
                    else
                    {
                         var result = keyValues[key];
                        if (result == null)
                            return key;

                        return (string)result; 

                    }
                }
                catch
                {
                    return key;
                }
            }
        }
    }
}

