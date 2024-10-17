namespace AKSoftware.Localization.MultiLanguages.CodeGeneration
{
    public interface IGenerateKeysService
    {
        void CreateStaticConstantsKeysFile(string namespaceName, string className, string filePath);
        void CreateEnumKeysFile(string namespaceName, string enumName, string filePath);
    }
}
