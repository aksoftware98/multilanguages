namespace AKSoftware.Localization.MultiLanguages
{
    public interface ICreateCodeLogic
    {
        void CreateStaticConstantsKeysFile(string namespaceName, string className, string filePath);
        void CreateEnumKeysFile(string namespaceName, string enumName, string filePath);
    }
}
