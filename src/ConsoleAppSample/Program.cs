// See https://aka.ms/new-console-template for more information
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Providers;

Console.WriteLine("Hello, World!");

ILanguageContainerService language = new LanguageContainer(new FolderResourceKeysProvider("Resources"));

var value = language["HelloWorld"];