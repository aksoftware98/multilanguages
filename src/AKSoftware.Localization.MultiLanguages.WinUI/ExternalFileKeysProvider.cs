using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly StorageFolder _localizationFolder;

        private ExternalFileKeysProvider(string resourceFolderName, StorageFolder localizationFolder)
        {
            _resourceFolderName = resourceFolderName ?? throw new ArgumentNullException(nameof(resourceFolderName));
            _localizationFolder = localizationFolder ?? throw new ArgumentNullException(nameof(localizationFolder));
            Log($"Initialized with folder '{_resourceFolderName}' at '{_localizationFolder.Path}'");
        }

        /// <summary>
        /// Creates an instance of <see cref="ExternalFileKeysProvider"/> that loads files from the app local folder.
        /// For packaged apps this resolves to LocalState\{resourceFolderName}; for unpackaged apps it falls back to %LocalAppData%\{AssemblyName}\{resourceFolderName}.
        /// </summary>
        public static Task<ExternalFileKeysProvider> CreateFromLocalFolderAsync(Assembly resourcesAssembly, string resourceFolderName = "Localization")
        {
            return CreateAsync(resourcesAssembly, resourceFolderName, LocalizationFolderType.LocalFolder);
        }

        /// <summary>
        /// Creates an instance of <see cref="ExternalFileKeysProvider"/> that loads files from the app installation folder.
        /// For packaged apps this resolves to Package.Current.InstalledLocation\{resourceFolderName}; for unpackaged apps it falls back to AppContext.BaseDirectory\{resourceFolderName}.
        /// </summary>
        public static Task<ExternalFileKeysProvider> CreateFromInstallationFolderAsync(Assembly resourcesAssembly, string resourceFolderName = "Localization")
        {
            return CreateAsync(resourcesAssembly, resourceFolderName, LocalizationFolderType.InstallationFolder);
        }

        /// <summary>
        /// Creates an instance of <see cref="ExternalFileKeysProvider"/> that loads files from a fully qualified external path.
        /// </summary>
        public static async Task<ExternalFileKeysProvider> CreateFromExternalFolderAsync(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentNullException(nameof(folderPath));

            var localizationFolder = await ResolveLocalizationFolderAsync(null, folderPath, LocalizationFolderType.ExternalFolder);
            return new ExternalFileKeysProvider(folderPath, localizationFolder);
        }

        /// <summary>
        /// Creates an instance of ExternalFileKeysProvider asynchronously.
        /// This method must be called from a thread with WinRT context (typically the UI thread).
        /// Supports both packaged and unpackaged WinUI apps.
        /// </summary>
        public static async Task<ExternalFileKeysProvider> CreateAsync(Assembly resourcesAssembly, string resourceFolderName = "Localization", LocalizationFolderType localizationFolderType = LocalizationFolderType.LocalFolder)
        {
            _ = resourcesAssembly ?? throw new ArgumentNullException(nameof(resourcesAssembly));
            if (string.IsNullOrWhiteSpace(resourceFolderName))
                throw new ArgumentNullException(nameof(resourceFolderName));

            var localizationFolder = await ResolveLocalizationFolderAsync(resourcesAssembly, resourceFolderName, localizationFolderType);
            return new ExternalFileKeysProvider(resourceFolderName, localizationFolder);
        }

        private static async Task<StorageFolder> ResolveLocalizationFolderAsync(Assembly resourcesAssembly, string resourceFolderName, LocalizationFolderType localizationFolderType)
        {
            Log($"CreateAsync called. Assembly='{resourcesAssembly?.GetName().Name}', ResourceFolder='{resourceFolderName}', FolderType={localizationFolderType}");

            if (localizationFolderType == LocalizationFolderType.LocalFolder)
            {
                try
                {
                    Log($"Trying packaged local folder '{resourceFolderName}' via ApplicationData.Current.LocalFolder.");
                    var localizationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(resourceFolderName, CreationCollisionOption.OpenIfExists);
                    Log($"Using packaged local folder '{localizationFolder.Path}'.");
                    return localizationFolder;
                }
                catch (InvalidOperationException ex)
                {
                    var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var appName = resourcesAssembly.GetName().Name;
                    var localPath = Path.Combine(localAppData, appName, resourceFolderName);
                    Log($"Packaged local folder unavailable ({ex.Message}). Falling back to unpackaged local path '{localPath}'. Exists={Directory.Exists(localPath)}");

                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                        Log($"Created unpackaged local folder '{localPath}'.");
                    }

                    var localizationFolder = await StorageFolder.GetFolderFromPathAsync(localPath);
                    Log($"Using unpackaged local folder '{localizationFolder.Path}'.");
                    return localizationFolder;
                }
            }

            if (localizationFolderType == LocalizationFolderType.InstallationFolder)
            {
                try
                {
                    Log($"Trying packaged installation folder '{resourceFolderName}' via Package.Current.InstalledLocation.");
                    var localizationFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(resourceFolderName);
                    Log($"Using packaged installation folder '{localizationFolder.Path}'.");
                    return localizationFolder;
                }
                catch (InvalidOperationException ex)
                {
                    var installPath = Path.Combine(AppContext.BaseDirectory, resourceFolderName);
                    Log($"Packaged installation folder unavailable ({ex.Message}). Falling back to unpackaged install path '{installPath}'. Exists={Directory.Exists(installPath)}");

                    if (!Directory.Exists(installPath))
                    {
                        Directory.CreateDirectory(installPath);
                        Log($"Created unpackaged installation folder '{installPath}'.");
                    }

                    var localizationFolder = await StorageFolder.GetFolderFromPathAsync(installPath);
                    Log($"Using unpackaged installation folder '{localizationFolder.Path}'.");
                    return localizationFolder;
                }
            }

            if (localizationFolderType == LocalizationFolderType.ExternalFolder)
            {
                if (!Path.IsPathFullyQualified(resourceFolderName))
                    throw new ArgumentException("When using ExternalFolder, resourceFolderName must be a fully qualified path.", nameof(resourceFolderName));

                Log($"Using external folder path '{resourceFolderName}'. Exists={Directory.Exists(resourceFolderName)}");

                if (!Directory.Exists(resourceFolderName))
                {
                    Directory.CreateDirectory(resourceFolderName);
                    Log($"Created external folder '{resourceFolderName}'.");
                }

                var localizationFolder = await StorageFolder.GetFolderFromPathAsync(resourceFolderName);
                Log($"Using external folder '{localizationFolder.Path}'.");
                return localizationFolder;
            }

            throw new ArgumentOutOfRangeException(nameof(localizationFolderType));
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

            Log($"GetKeys requested for culture '{cultureName}' from '{_localizationFolder.Path}'.");

            var fileName = GetFileName(cultureName);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Log($"No localization file found for culture '{cultureName}' in '{_localizationFolder.Path}'.");
                return null;
            }

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
            Log($"Resolved culture '{cultureName}' to file '{fileName ?? "<none>"}'.");
            return fileName;
        }

        private StorageFile GetFile(string fileName)
        {
            Log($"Opening localization file '{fileName}' from '{_localizationFolder.Path}'.");
            return _localizationFolder.GetFileAsync(fileName).AsTask().GetAwaiter().GetResult();
        }

        private string[] GetLanguageFileNames()
        {
            var files = _localizationFolder.GetFilesAsync().AsTask().GetAwaiter().GetResult();
            var fileNames = files.Select(file => file.Name).ToArray();
            Log($"Discovered files in '{_localizationFolder.Path}': {string.Join(", ", fileNames)}");
            return fileNames;
        }

        private Keys InternalGetKeys(string fileName)
        {
            var localizationFile = GetFile(fileName);
            var keys = FileIO.ReadTextAsync(localizationFile).AsTask().GetAwaiter().GetResult();
            Log($"Loaded localization file '{localizationFile.Path}'.");
            return new Keys(keys);
        }

        private static void Log(string message)
        {
            Debug.WriteLine($"[ExternalFileKeysProvider] {message}");
            Trace.WriteLine($"[ExternalFileKeysProvider] {message}");
        }
    }
}
