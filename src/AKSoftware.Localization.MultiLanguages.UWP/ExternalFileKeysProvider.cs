using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;

namespace AKSoftware.Localization.MultiLanguages.UWP
{

   
    public class ExternalFileKeysProvider : KeysProvider
    {

        public ExternalFileKeysProvider(Assembly resourcesAssembly, string resourceFolderName = "Localization", LocalizationFolderType localizationFolderType = LocalizationFolderType.LocalFolder) : base(resourcesAssembly, resourceFolderName)
        {
            LocalizationFolderType = localizationFolderType;
        }

        private LocalizationFolderType LocalizationFolderType { get;  }

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
                            // TODO: better error handling}
                            var task = Task.Run(async () =>
                                await ApplicationData.Current.LocalFolder.GetFolderAsync(ResourceFolderName));
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
                                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(
                                    ResourceFolderName));
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

        protected override string GetFileName(string cultureName)
        {
            var files = GetLanguageFileNames();
            var fileName =  files.SingleOrDefault(file =>
                                        file.Contains(cultureName) && 
                                        (file.Contains($"{cultureName}.yml") || file.Contains($"{cultureName}.yaml")));
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

        protected override string[] GetLanguageFileNames()
        {
            var task = Task.Run(async () => await LocalizationFolder.GetFilesAsync());
            if (!task.IsFaulted)
            {
                var files = task.Result;
                return files.Select(file => file.Name).ToArray();
            }
            throw task.Exception;
           
        }

        protected override Keys InternalGetKeys(string fileName)
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
