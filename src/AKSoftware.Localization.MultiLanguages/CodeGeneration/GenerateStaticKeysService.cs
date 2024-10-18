using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages.CodeGeneration
{
    /// <summary>
    /// Generate a class that contains all the keys flat as contains to be used in your application once needed.
    /// 
    /// </summary>
    public class GenerateStaticKeysService : IGenerateKeysService
    {
        private readonly ILanguageContainerService _languageContainer;

        public GenerateStaticKeysService(ILanguageContainerService languageContainer)
        {
            _languageContainer = languageContainer;
        }

        public void CreateStaticConstantsKeysFile(string namespaceName, string className, string filePath)
        {
            if (string.IsNullOrEmpty(namespaceName))
                throw new ArgumentNullException(nameof(namespaceName));
            if (string.IsNullOrEmpty(className))
                throw new ArgumentNullException(nameof(className));
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendGeneratedByToolComment();
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic static class {className}");
            sb.AppendLine("\t{");
            List<string> keys = _languageContainer.GetKeys().OrderBy(o => o).ToList();
            foreach (var key in keys)
            {
                string constantName = key.Replace(":", string.Empty);
                sb.AppendLine($"\t\tpublic const {constantName} = \"{key}\";");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText(filePath, sb.ToString());
        }

        public void CreateEnumKeysFile(string namespaceName, string enumName, string filePath)
        {
            if (string.IsNullOrEmpty(namespaceName))
                throw new ArgumentNullException(nameof(namespaceName));
            if (string.IsNullOrEmpty(enumName))
                throw new ArgumentNullException(nameof(enumName));
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendGeneratedByToolComment();
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic enum {enumName}");
            sb.AppendLine("\t{");
            List<string> keys = _languageContainer.GetKeys().OrderBy(o => o).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                string enumValue = key.Replace(":", string.Empty);

                if (key != enumValue)
                {
                    sb.AppendLine($"\t\t[Description(\"{key}\")]");
                }

                if (i == keys.Count - 1)
                {
                    sb.AppendLine($"\t\t{enumValue}");
                }
                else
                {
                    sb.AppendLine($"\t\t{enumValue},");
                }
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText(filePath, sb.ToString());
        }


        
    }
}
