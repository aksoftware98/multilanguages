using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AKSoftware.Localization.MultiLanguages.Providers;
using Windows.Storage;

namespace AKSoftware.Localization.MultiLanguages.WinUI
{
    public class ExternalFileKeysProvider : BaseKeysProvider
    {
        private readonly string _resourceFolderName;

        public ExternalFileKeysProvider(Assembly resourcesAssembly, string resourceFolderName = "Localization", LocalizationFolderType localizationFolderType = LocalizationFolderType.LocalFolder)
        {
            _ = resourcesAssembly ?? throw new ArgumentNullException(nameof(resourcesAssembly));
            if (string.IsNullOrWhiteSpace(resourceFolderName))
                throw new ArgumentNullException(nameof(resourceFolderName));

            _resourceFolderName = resourceFolderName;
            LocalizationFolderType = localizationFolderType;
        }

        private LocalizationFolderType LocalizationFolderType { get; }

        private StorageFolder _localizationFolder;
        private StorageFolder LocalizationFolder
        {
            get
            {
                if (_localizationFolder == null)
                {
                    switch (LocalizationFolderType)
                    {
                        case LocalizationFolderType.LocalFolder:
                        {
                            var task = Task.Run(async () =>
                                await ApplicationData.Current.LocalFolder.GetFolderAsync(_resourceFolderName));
                            if (!task.IsFaulted)
                            {
                                _localizationFolder = task.Result;
                            }
                            else
                            {
                                throw task.Exception;
                            }

                            break;
                        }
                        case LocalizationFolderType.InstallationFolder:
                        {
                            var task = Task.Run(async () =>
                                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(_resourceFolderName));
                            if (!task.IsFaulted)
                            {
                                _localizationFolder = task.Result;
                            }
                            else
                            {
                                throw task.Exception;
                            }

                            break;
                        }
                    }
                }

                return _localizationFolder;
            }
        }

        public override Keys GetKeys(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            return GetKeys(cultureInfo.Name);
        }

        public override Keys GetKeys(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
                throw new ArgumentNullException(nameof(cultureName));

            var fileName = GetFileName(cultureName);
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            return InternalGetKeys(fileName);
        }

        public override IEnumerable<CultureInfo> RegisteredLanguages
        {
            get
            {
                var cultures = new List<CultureInfo>();
                var languageFileNames = GetLanguageFileNames();

                foreach (var fileName in languageFileNames)
                {
                    if (YamlFilePattern.IsMatch(fileName))
                    {
                        var cultureName = Path.GetFileNameWithoutExtension(fileName);
                        cultures.Add(new CultureInfo(cultureName));
                    }
                }

                return cultures;
            }
        }

        private string GetFileName(string cultureName)
        {
            var files = GetLanguageFileNames();
            var fileName = files.SingleOrDefault(file =>
                file.Contains(cultureName) &&
                (file.Contains(cultureName + ".yml") || file.Contains(cultureName + ".yaml")));
            return fileName;
        }

        private StorageFile GetFile(string fileName)
        {
            var task = Task.Run(async () => await LocalizationFolder.GetFileAsync(fileName));
            if (!task.IsFaulted)
            {
                return task.Result;
            }

            throw task.Exception;
        }

        private string[] GetLanguageFileNames()
        {
            var task = Task.Run(async () => await LocalizationFolder.GetFilesAsync());
            if (!task.IsFaulted)
            {
                var files = task.Result;
                return files.Select(file => file.Name).ToArray();
            }

            throw task.Exception;
        }

        private Keys InternalGetKeys(string fileName)
        {
            var localizationFile = GetFile(fileName);
            var task = Task.Run(async () => await FileIO.ReadTextAsync(localizationFile));
            if (!task.IsFaulted)
            {
                var keys = task.Result;
                return new Keys(keys);
            }

            throw task.Exception;
        }
    }
}
