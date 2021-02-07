using System.IO;

namespace AKSoftware.Localization.MultiLanguages
{
    public interface IFileProvider
    {
        Stream GetFileAsStream(string fileName);
        string[] GetLanguageFileNames();
    }
}
