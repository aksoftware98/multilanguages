
  
  

# AKSoftware.Localization.MultiLanguages
### Version 5.9 is here [Check out What's New](#whats-new-in-version-590)
[![Build Badge](https://aksoftware98.visualstudio.com/AkMultiLanguages/_apis/build/status/aksoftware98.multilanguages?branchName=master)](https://aksoftware98.visualstudio.com/AkMultiLanguages/_build/latest?definitionId=4&branchName=master)

  

![Nuget](https://img.shields.io/nuget/dt/AKSoftware.Localization.MultiLanguages?color=nuget&label=Nuget&style=plastic)

  

Build awesome .NET applications that supports more than 69+ languages with just a few lines of code, in addition to an easy translation tool that helps you translate all your content to any language you want with just one click

Could be used for all type of .NET Apps (Blazor, UWP, Xamarin, Windows, ASP.NET Core MVC, Razor Pages ....)

https://akmultilanguages.azurewebsites.net

Build with Love by Ahmad Mozaffar

http://ahmadmozaffar.net

  

## YouTube Session

https://youtu.be/Xz68c8GBYz4

  

![Simple UI supports German](https://github.com/aksoftware98/multilanguages/blob/master/Example/BlazorWasmMultiLanguages/BlazorWasmMultiLanguages/wwwroot/German.png?raw=true)

  

![Blazor UI with Japanease](https://raw.githubusercontent.com/aksoftware98/multilanguages/master/Example/BlazorWasmMultiLanguages/BlazorWasmMultiLanguages/wwwroot/Japan.png)


# What's new in Version 5.9.0
Version 5.9.0 with two big achievements:
1. The performance of the library has been improved by 5x especially while retrieving nested keys, due to eliminate the usage of JSON in some places and depend efficiently on the YAML library. 
2. The ability to store the Resources in an external folder with a specified path that you can define
To use this feature without using Dependency Injection 
You can refer to this sample file here:
[Program.cs sample to fetch keys from folder](https://github.com/aksoftware98/multilanguages/blob/master/src/ConsoleAppSample/Program.cs)

If you are using dependency injection you can use the newly used method: 
``` C#
// For .NET projects consider the following method
services.AddLanguageContainerFromFolder("Resouces", CultureInfo.GetCultureInfo("en-US")); 

// For Blazor Server
services.AddLanguageContainerFromFolderForBlazorServer("Resouces", CultureInfo.GetCultureInfo("en-US")); 
```

**Keep in mind, the folder of the resources has to be shipped with your project**

  # What's new in Version 5.8.0 
  Finally **Blazor Server** is here you can get started now.
  The **only** change that you need to do that is different from others is in your **program.cs** file make sure to use the following function to register the Language Container
``` C#
...
// Register the language container for Blazor Server
builder.Services.AddLanguageContainerForBlazorServer<EmbeddedResourceKeysProvider>(Assembly.GetExecutingAssembly(), "Resources");
...
```
 Check out the following [Blazor Server Sample](https://github.com/aksoftware98/multilanguages/tree/master/src/BlazorServerLocalizationSample)

# UWP Support in Version 5.0.0

Special thanks for the contributor [Michael Gerfen](https://github.com/gerfen) for updating the library to add support to the UWP.

This version contains a major update that adds support for a new package under the name AKSoftware.Localization.MultiLangauges.UWP you can install it from Nuget with the following command

  

``` PS

Install-Package AKSoftware.Localization.MultiLangauges.UWP

```

  

# What's new in Version 5.0.0

In the latest version of the library because right it started to support UWP and not only Blazor a new interface and abstract type has been introduced that allows you to easily create a keys provider to fetch your keys not only from the embedded resources, also from any source you would like (External folder, FTP, Azure Blob Storage ...etc)

By default there is the ***EmbeddedResourceKeysProvider*** to fetch the files from the resources and you can create your own by inhereting from the interface ***IKeysProvider***

More about the implementation in the Wiki soon

  

# Getting Started

  

For Nuget Package Manager install the package

(Nuget Package Manager Console)

``` PS

Install-Package AKSoftware.Localization.MultiLanguages

```

(Using dotNet CLI)

``` CLI

dotnet add package AKSoftware.Localization.MultiLanuages

```

**For Blazor** additional package is required that helps managing the state of the component when changing the language

(Nuget Package Manager Console)

``` PS

Install-Package AKSoftware.Localization.MultiLanguages.Blazor

```

(Using dotNet CLI)

``` CLI

dotnet add package AKSoftware.Localization.MultiLanuages.Blazor

```

  
  

## Create the Resources Folder

  

Inside your project create a folder called "Resources"

and inside it create a file called "*en-US.yml*" which is a YAML file

then set your keywords inside the file like this

``` YAML

HelloWorld: Hello World

Welcome: Welcome

...

```

> We chose YAML files because it's very light comparing it to XML or JSON and make the output dll very small, in addition to that it's much way faster in serialization and deserialization

  

## Set the build action of the file to EmbeddedResource

  

Select the file in the Solution Explorer window and from the properties window set the build action property to "Embeded Resources"

  

## Translate the file

  

Visit the online translation tool on the following link

https://akmultilanguages.azurewebsites.net

  

Go to translate app page

  

Upload your YAML file and click submit

All the languages will be available with just one click - install all the languages you want to support in your application

  

## Import the files to the Resources folder

  

Import the files to the resources folder you have just created and set the build action property for them as Embedded Resources also

  

## Coding time

### Blazor Demo:

  

Go to **program.cs** and register the Language Container Service in the Dependency Injection container

Import the library

``` C#

using  AKSoftware.Localization.MultiLanguages

```

  

Register the service

``` C#

// Specify the assembly that has the langauges files, in this situation it's the current assembly

builder.Services.AddLanguageContainer<EmbeddedResourceKeysProvider>(Assembly.GetExecutingAssembly());

// You can specify the default culture of the project like this

// builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("fr-Fr"));

```

**Note:**

If you don't specify a default culture the library will try to find the file that matches the culture of the current user, if it's not existing it will try to find any file that matches the same language, then if it's not there it will try to find the English file then the first file in the folder, otherwise it will throw an exception

  
  

# Start dealing with components

In the _imports.razor file make sure to add the following namespaces

``` C#

using  AKSoftware.Localization.MultiLanguages

using  AKSoftware.Localization.MultiLanguages.Blazor

```

With in your components that you want to localize inject the service

``` C#

@inject  ILanguageContainerService  languageContainer

```

And start getting the values from your files just like this

``` Razor

<h1>@languageContainer.Keys["HelloWorld"]</h1>

```

  

And to be able to get the state updated for each component that contains localized text call the extension method in the OnInitialized or OnInitializedAsync overriden methods for each component as following

``` C#

protected  override  void  OnInitialized()

{

// This will make the current component gets updated whenever you change the language of the application

languageContainer.InitLocalizedComponent(this);

}

```

  

## Change the language from the UI

  

You are able to change the language and choose any language you have added from the UI like this

Inject the service in the component

``` C#

@inject  ILanguageContainerService  languageContainer

```

Add a button and set the @onclick method

``` Razor

<button @onclick="SetFrench">French</button>

@code

{

void SetFrench()

{

languageContainer.SetLanguage(System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));

}

}

```

  

## Interpolation Feature

Starting from version 4.0 now there is the ability to create dynamic values to replace their values at runtime using Interpolation feature:

Following you can see how to use this feature

  

Language File en-US:

```YAML

Welcome: Welcome {username} to our system {version}

```

  

In C# to replace the value of username and version parameters at runtime you can use the new indexer that allows to pass an object for with these values as following:

  

```C#

_language["Welcome", new

{

Username = "aksoftware98",

Version = "v4.0"

}]

```

  

## Check the Sample Folder

Check the sample project here to see how to develop a full Blazor WebAssembly project with storing the last selected language with more than 8 languages available for one UI:

[Full Blazor WASM Sample](https://github.com/aksoftware98/multilanguages/tree/master/src/BlazorAKLocalization)

# Upcoming in Version 6.0
We are currently working on version 6.  Here are the upcoming features.

* [Specify the assembly by name](#specify-the-assembly-by-name)
* [Get all the keys for the current culture](#get-all-the-keys-for-the-current-culture)
* [Use an Enum as a translation key](#use-an-enum-as-a-translation-key)
* [Loop through the key values for the current culture](#loop-through-the-key-values-for-the-current-culture)
* [Get all the registered languages](#get-all-the-registered-languages)
* [Generate a Static Constants Keys File](#generate-a-static-constants-keys-file)
* [Generate an Enum Keys File](#generate-an-enum-keys-file)
* [Verify All Source Code Files Are Localized](#verify-all-source-code-files-are-localized)
* [Verify All Keys Can Be Found](#verify-all-keys-can-be-found)
* [Verify No Unused Keys](#verify-no-unused-keys)
* [Verify No Duplicate Keys](#verify-no-duplicate-keys)

## Specify the assembly by name
If you have multiple projects in your Visual Studio Solution that depend upon language translation, as of version 6.0 and higher you can specify the assembly by name.  Place your resources in a project that can be used by the other projects in your Solution.

Example Usage
```C#
string assemblyName = "MyCompany.MyProject.Common";
EmbeddedResourceKeysProvider keysProvider = new EmbeddedResourceKeysProvider(assemblyName, "Resources");
LanguageContainer service = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
```

## Get all the keys for the current culture

```C#
List<string> keys = _language.GetKeys();
```

## Loop through the key values for the current culture.

```C#
foreach (KeyValuePair<object, object> keyValue in _service.Keys)
{
	Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
}
```

## Get all the registered languages

```C#
IEnumerable<CultureInfo> registeredLanguages = _language.RegisteredLanguages;
```

Full example with a drop-down that is bound to the languages.

```C#
@page "/"
@using System.Globalization

<select @bind="SelectedCulture" @bind:event="onchange">
    @foreach (var culture in Cultures)
    {
        <option value="@culture.Name">@culture.EnglishName</option>
    }
</select>

@code {
    [Inject] private ILanguageContainerService _language { get; set; }
	
    public IEnumerable<CultureInfo> Cultures { get; set; } = new List<CultureInfo>();
    
    private string _selectedCulture;
    public string SelectedCulture
    {
        get => _selectedCulture;
        set
        {
            if (_selectedCulture != value)
            {
                _selectedCulture = value;
                _language.SetLanguage(CultureInfo.GetCultureInfo(value));
            }
        }
    }

    protected override void OnInitialized()
    {
        _language.InitLocalizedComponent(this);
		
        // Initialize the Cultures list here if not already populated
        if (Cultures.Count == 0)
        {
            Cultures = _language.GetRegisteredLanguages();
        }

        // Set initial selected culture
        _selectedCulture = _language.CurrentCulture.Name;
    }
}
```
## Use an Enum as a translation key
The name of the enum will be used as the key.  If there is a Description attribute, the Description will be used as the key. Note, as of Version 6.0 and higher, the library now has the ability to generate a LanguageKeys Enum file.  

Example Enum
```C#
	public enum LanguageKeys
	{
		[Description("HomePage:Title")]
		HomePageTitle,
		FirstName
	}
```

Example Usage
``` Razor
<h1>@languageContainer.Keys[LanguageKeys.HomePageTitle]</h1>
```


## Generate a Static Constants Keys File
We are currently working on a CLI but you can also create a static constants file using this method.

```C#
var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
var languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
var createCodeLogic = new CreateCodeLogic(languageContainer);
string namespace = "MyCompany.Project";
string className = "LanguageKeys";
string filePath = @"c:\somedirectory\LanguageKeys.cs"
createCodeLogic.CreateStaticConstantsKeysFile(namespaceName, className, filePath);
```

This will produce a file like this.

```C#
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//      
//     For more information see: https://github.com/aksoftware98/multilanguages
// </auto-generated>
//------------------------------------------------------------------------------
namespace MyCompany.Project
{
	public static class LanguageKeys
	{
	    public const HomePageTitle = "HomePage:Title";
		public const FirstName = "FirstName";
	}
}
```

Here is an example of the usage.
``` Razor
<h1>@languageContainer.Keys[LanguageKeys.FirstName]</h1>
```

## Generate an Enum Keys File
We are currently working on a CLI but you can also create an enum keys file using this method.

```C#
var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
var languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
var createCodeLogic = new CreateCodeLogic(languageContainer);
string namespace = "MyCompany.Project";
string enumName = "LanguageKeys";
string filePath = @"c:\somedirectory\LanguageKeys.cs"
createCodeLogic.CreateEnumKeysFile(namespaceName, enumName, filePath);
```

This will produce a file like this.

```C#
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//      
//     For more information see: https://github.com/aksoftware98/multilanguages
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel;
namespace MyCompany.Project
{
	public enum LanguageKeys
	{
        [Description("HomePage:Title")]
		HomePageTitle,
		FirstName
	}
}
```

Here is an example of the usage.
``` Razor
<h1>@languageContainer.Keys[LanguageKeys.FirstName]</h1>
```
## Validation
In order to keep you project localized, there are several different tests that an be performed.  

## Verify All Source Code Files Are Localized
As you are adding and changing Razor files in your your project, you can verify that all source code files have been localized. If the result is empty then everything has been localized.

Example:

```C#
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
	parms.ResourceFilePath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Resources", "en-US.yml");
	parms.KeyReference = "Language";

	//Act   
	ParseCodeLogic logic = new ParseCodeLogic();
	List<ParseResult> parseResults = logic.GetLocalizableStrings(parms);

	//Assert
	if (parseResults.Any())
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("Not all source code files are localized. See documentation here: https://github.com/aksoftware98/multilanguages");
		foreach (var parseResult in parseResults)
		{
			sb.AppendLine($"{Path.GetFileName(parseResult.FilePath)} | {parseResult.MatchValue} | {parseResult.LocalizableString}");
		}

		Assert.Fail(sb.ToString());
	}
}
```

## Verify All Keys Can Be Found
You can verify that there is not a typo in your Razor file for the localization key.  When the list is not blank there are typos.   

Example:

```C#
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
	parms.ResourceFilePath = Path.Combine(solutionPath, "BlazorServerLocalizationSample", "Resources", "en-US.yml");
	parms.KeyReference = "Language";

	//Act
	ParseCodeLogic logic = new ParseCodeLogic();
	IEnumerable<ParseResult> parseResults = logic.GetExistingLocalizedStrings(parms).Where(o => String.IsNullOrEmpty(o.LocalizableString));

	//Assert
	if (parseResults.Any())
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("Not all keys can be found in the resource file.  See documentation here: https://github.com/aksoftware98/multilanguages");
		foreach (var parseResult in parseResults)
		{
			sb.AppendLine($"{parseResult.FilePath} | {parseResult.MatchValue}");
		}

		Assert.Fail(sb.ToString());
	}
}
```

## Verify No Unused Keys
Detect if you have keys in your en-US.yml file that are not being used in your razor files.

Example:

```C#
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
```

## Verify No Duplicate Keys
In this situation, there are strings that need to be localized but it would result in duplicate keys if automatically created.  You might need to manually create the key and values.

Example
```C#
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
```

# Thanks for the awesome contributors

<a  href="https://github.com/aksoftware98/multilanguages/graphs/contributors">

<img  src="https://contrib.rocks/image?repo=aksoftware98/multilanguages"  />

</a>
