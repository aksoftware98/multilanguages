using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public class VerifyLocalization
    {
        /// <summary>
        /// If this test is failing it means that there are new strings in your razor file or in your model file Required Attribute that need to be localized.
        /// </summary>
        [Fact]
        public void VerifyAllSourceCodeFilesAreLocalized()
        {
            //Arrange
            ParseParms parms = new ParseParms();
            string solutionPath = TestHelper.GetSolutionPath();
            string pagesPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Pages");
            string sharedPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Shared");
            parms.SourceDirectories = new List<string> { pagesPath, sharedPath };
            parms.WildcardPatterns = new List<string>() { "*.razor" };
            parms.ExcludeDirectories = new List<string>();
            parms.ExcludeFiles = new List<string>();
            parms.ResourceFilePath = Path.Combine(solutionPath, "BlazorServerLocalizationSample",
                "Resources", "en-US.yml");
            parms.KeyReference = "Language";
            parms.RemoveLocalizedKeys = true;

            //Act   
            ParseCodeLogic logic = new ParseCodeLogic();
            List<ParseResult> parseResults = logic.GetLocalizableStrings(parms);

            //Assert
            if (parseResults.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(
                    "Not all source code files are localized. See documentation here: https://github.com/aksoftware98/multilanguages");
                foreach (var parseResult in parseResults)
                {
                    sb.AppendLine(
                        $"{Path.GetFileName(parseResult.FilePath)} | {parseResult.MatchValue} | {parseResult.LocalizableString}");
                }

                Assert.Fail(sb.ToString());
            }
        }

        /// <summary>
        /// If this test is failing it means that you manually typed in a key in your razor file,
        /// and it does not exist in the en-US.yml file, or you deleted a key value pair in the en-Us.yml file that was in use.
        /// </summary>
        [Fact]
        public void VerifyAllKeysCanBeFound()
        {
            //Arrange
            ParseParms parms = new ParseParms();
            string solutionPath = TestHelper.GetSolutionPath();
            string pagesPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Pages");
            string sharedPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Shared");
            parms.SourceDirectories = new List<string> { pagesPath, sharedPath };
            parms.WildcardPatterns = new List<string>() { "*.razor" };
            parms.ExcludeDirectories = new List<string>();
            parms.ExcludeFiles = new List<string>();
            parms.ResourceFilePath = Path.Combine(solutionPath, "BlazorServerLocalizationSample",
                "Resources", "en-US.yml");
            parms.KeyReference = "Language";

            //Act
            ParseCodeLogic logic = new ParseCodeLogic();
            IEnumerable<ParseResult> parseResults = logic.GetExistingLocalizedStrings(parms)
                .Where(o => String.IsNullOrEmpty(o.LocalizableString));

            //Assert
            if (parseResults.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(
                    "Not all keys can be found in the resource file.  See documentation here: https://github.com/aksoftware98/multilanguages");
                foreach (var parseResult in parseResults)
                {
                    sb.AppendLine($"{parseResult.FilePath} | {parseResult.MatchValue}");
                }

                Assert.Fail(sb.ToString());
            }
        }

        /// <summary>
        /// If this test is failing, it means that you have keys in your en-US.yml file that are not being used in your razor files.
        /// </summary>
        [Fact]
        public void VerifyNoUnusedKeys()
        {
            //Arrange
            ParseParms parms = new ParseParms();
            string solutionPath = TestHelper.GetSolutionPath();
            string pagesPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Pages");
            string sharedPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Shared");
            parms.SourceDirectories = new List<string> { pagesPath, sharedPath };
            parms.WildcardPatterns = new List<string>() { "*.razor" };
            parms.ExcludeDirectories = new List<string>();
            parms.ExcludeFiles = new List<string>();
            parms.ResourceFilePath = Path.Combine(solutionPath, "BlazorServerLocalizationSample",
                "Resources", "en-US.yml");
            parms.KeyReference = "Language";

            //Act
            ParseCodeLogic logic = new ParseCodeLogic();
            List<string> unusedKeys = logic.GetUnusedKeys(parms);

            //Assert
            if (unusedKeys.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(
                    "There are unused keys in the resource file.  See documentation here: https://github.com/aksoftware98/multilanguages");
                foreach (var unusedKey in unusedKeys)
                {
                    sb.AppendLine(unusedKey);
                }

                Assert.Fail(sb.ToString());
            }
        }


        /// <summary>
        /// If this test is failing it means that there are new strings that need to be localized
        /// and if they were to be created automatically, there would be the same key that have different values
        /// </summary>
        [Fact]
        public void VerifyNoDuplicateKeys()
        {
            //Arrange
            ParseParms parms = new ParseParms();
            string solutionPath = TestHelper.GetSolutionPath();
            string pagesPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Pages");
            string sharedPath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Shared");
            parms.SourceDirectories = new List<string> { pagesPath, sharedPath };
            parms.WildcardPatterns = new List<string>() { "*.razor" };
            parms.ExcludeDirectories = new List<string>();
            parms.ExcludeFiles = new List<string>();
            parms.ResourceFilePath = Path.Combine(solutionPath, "BlazorServerLocalizationSample",
                "Resources", "en-US.yml");
            parms.KeyReference = "Language";

            //Act   
            ParseCodeLogic logic = new ParseCodeLogic();
            Dictionary<string, List<string>> failedKeys = logic.GetDuplicateKeys(parms);

            //Assert
            if (failedKeys.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(
                    "Missing localized values would have duplicate keys.  See documentation here: https://github.com/aksoftware98/multilanguages");
                foreach (var failedKey in failedKeys)
                {
                    foreach (var item in failedKey.Value)
                    {
                        sb.AppendLine($"{failedKey.Key} : {item}");
                    }
                }

                Assert.Fail(sb.ToString());
            }
        }
    }
}
