# AKSoftware.Localization.MultiLanguages
## Blazor Localization
## ASP.NET Localization
## .NET Localization

### v6.0.0-alpha
[![Build Badge](https://aksoftware98.visualstudio.com/AkMultiLanguages/_apis/build/status/aksoftware98.multilanguages?branchName=master)](https://aksoftware98.visualstudio.com/AkMultiLanguages/_build/latest?definitionId=4&branchName=master)

  

![Nuget](https://img.shields.io/nuget/dt/AKSoftware.Localization.MultiLanguages?color=nuget&label=Nuget&style=plastic)

  <img width="100" src="https://github.com/aksoftware98/multilanguages/blob/master/src/AKSoftware.Localization.MultiLanguages/AkMultiLanguages.png?raw=true" />
    <img width="100" src="https://github.com/aksoftware98/multilanguages/blob/master/src/AKSoftware.Localization.MultiLanguages.Blazor/AkMultiLanguages.png?raw=true" />

The most advanced .NET localization package for your projects; light, fast, super easy to use, and much more.
Making your app available in many languages is now a simple straightforward process, *AKSoftware.Localization.Multilanguages* offers the ultimate solution.  

Why MultiLanguages is better:

| Feature                                                                        | .NET Localization  | AKSoftware.Localization.MultiLanguages |
|--------------------------------------------------------------------------------|:------------------:|:--------------------------------------:|
| Memory Heavy, Hard to Maintain RESX Resource Files                             | :heavy_check_mark: |                                        |
| Less Memory, Easy to Maintain YAML Resource Files                              |                    | :heavy_check_mark:                     |
| Generate English Resource YAML File from Localizable Strings from your UI Code |                    | :heavy_check_mark:                     |
| Replace Localizable Strings with Variables                                     |                    | :heavy_check_mark:                     |
| Data Attribute Localization                                                    | :heavy_check_mark: | :heavy_check_mark:                     |
| Hierarchal Language Key Support                                                |                    | :heavy_check_mark:                     |
| Translate Resource Files into 69 Different Languages                           |                    | :heavy_check_mark:                     |
| String Interpolation                                                           |                    | :heavy_check_mark:                     |
| Get Registered Languages                                                       |                    | :heavy_check_mark:                     |
| Use Enum as Translation Key                                                    | :heavy_check_mark: | :heavy_check_mark:                     |
| Generate Enum Translation Key Code                                             |                    | :heavy_check_mark:                     |
| Source generator for injectable keys accessor service                          |                    | :heavy_check_mark:                     |
| Use Static Class as Translation Key                                            | :heavy_check_mark: | :heavy_check_mark:                     |
| Generate Static Class Translation Key Code                                     | :heavy_check_mark: | :heavy_check_mark:                     |
| Verify All Source Code Files are Localized                                     |                    | :heavy_check_mark:                     |
| Verify All Keys Can Be Found                                                   |                    | :heavy_check_mark:                     |
| Verify There Are No Unused Keys                                                |                    | :heavy_check_mark:                     |
| Verify There Are No Duplicate Keys                                             |                    | :heavy_check_mark:                     |


### Key Features

<img width="400" src="https://github.com/aksoftware98/multilanguages/blob/master/Images/v6/yaml-based.png?raw=true" />
<img width="400" src="https://github.com/aksoftware98/multilanguages/blob/master/Images/v6/hierarchy-friendly.png?raw=true" />
<img width="400" src="https://github.com/aksoftware98/multilanguages/blob/master/Images/v6/source-generator.png?raw=true" />
<img width="400" src="https://github.com/aksoftware98/multilanguages/blob/master/Images/v6/interpolation-support.png?raw=true" />


It can be used for all type of .NET Apps (Blazor, UWP, Xamarin, Windows, ASP.NET Core MVC, Razor Pages ....)

https://akmultilanguages.azurewebsites.net

Built with Love by Ahmad Mozaffar

http://ahmadmozaffar.net

  

## YouTube Session

https://youtu.be/Xz68c8GBYz4

  

![Simple UI supports German](https://github.com/aksoftware98/multilanguages/blob/master/Example/BlazorWasmMultiLanguages/BlazorWasmMultiLanguages/wwwroot/German.png?raw=true)

  

![Blazor UI with Japanease](https://raw.githubusercontent.com/aksoftware98/multilanguages/master/Example/BlazorWasmMultiLanguages/BlazorWasmMultiLanguages/wwwroot/Japan.png)


# Why YAML
Most common solution for multilanguage in .NET is the .resx resource files. .resx files are XML based so they are not too friendly to deal with and most likely a GUI tool is needed for keys management. XML is also huge and slower to parse. On the other hand, YAML is new, very fast to parse, and file structure is very simple and doesn't contain any single not-needed character which make the file size too small comparing to the XML one.
For modern SPA apps with Blazor WebAssembly for example, large language files with .resx might slow down the load time for the download.
YAML file structure allows for nested objects which a lovely feature you can take advantage of to build an organized language key-values files without long concatenated names.
Finally, due to the simplicity of YAML, it's makes it very easy to build automation on top of it like source generator and static classes creation.

# Features
AKSoftware.Localization.Multilanguage prvoides all the feature set needed for any multilanguage support like:
- Easy to get started.
- Online traslator tool to translate your files in one click for more 65 languages  https://akmultilanguages.azurewebsites.net
- Light and high-performant
- Blazor Server & WebAssembly support
- Out of the box state management for **Blazor** components
- Multiple language file sources (Files in folder or embedded files)
- String interpolation support
- Dynamically list all language keys
- Dynamically list all available langauges
- Dependency injection support
- Hierarcy language keys in YAML
- Code generators to generate full keys accessor service, static class with const strings, enums, and more..
- v6.1 will bring the localization assistant to localize existing apps with minimal effort.
- Full UWP support



# Getting Started

  

For Nuget Package Manager install the package

(Nuget Package Manager Console)

``` PS

Install-Package AKSoftware.Localization.MultiLanguages -Version 6.0.0-alpha

```

(Using dotNet CLI)

``` CLI

dotnet add package AKSoftware.Localization.MultiLanuages --version 6.0.0-alpha

```

**For Blazor** additional package is required that helps managing the state of the component when changing the language

(Nuget Package Manager Console)

``` PS

Install-Package AKSoftware.Localization.MultiLanguages.Blazor -Version 6.0.0-alpha

```

(Using dotNet CLI)

``` CLI

dotnet add package AKSoftware.Localization.MultiLanuages.Blazor --version 6.0.0-alpha

```

**For Source Generator** install:
(Nuget Package Manager Console)
  
``` PS

Install-Package AKSoftware.Localization.MultiLanguages.SourceGenerator -Version 6.0.0-alpha

```

(Using dotNet CLI)

``` CLI

dotnet add package AKSoftware.Localization.MultiLanuages.SourceGenerator--version 6.0.0-alpha

```

>**Source Generator Note**
> When using the source generator, the package will take care of setting the file as embedded resources, and it generates a new interface named **IKeysAccessor** this service wraps the access to all the keys and tested keys in your language files.
> 
> However, the package must be installed the project that contains the en-US.yml file, so if your solution is a single project you can directly install it in the same project. If your solution consists of multiple projects and you want to use the localization across all of them, make sure to create a seperate project for localization and reference it in your other projects.
  

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

>**Note**
>In case of using the Source Generator package, that will be taken care of automatically.

  

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

> **Source Generator Note** 
> In case of using the Source Generator and want to have direct access without using the ILanguageContainerService and provide the key as string make sure to also register the auto-generated interface instance in your dependency injection container:
> ``
> builder.Services.AddKeysAccessor();
> ``

  
  

# Start dealing with components

In the _imports.razor file make sure to add the following namespaces

``` C#

using  AKSoftware.Localization.MultiLanguages

using  AKSoftware.Localization.MultiLanguages.Blazor

```

With in your components that you want to localize inject the service

```csharp

@inject  ILanguageContainerService  languageContainer

```
or with Source Genertaor inject the keys accessor instead
```csharp
@inject IKeysAccessor KeysAccessor
```

And start getting the values from your files just like this

```razor
@* Without Source Generator *@
<h1>@languageContainer.Keys["HelloWorld"]</h1>

@* With Source Generator *@
<h1>@KeysAccessor.HelloWorld</h1>

```

  

And to be able to get the state updated for each component that contains localized text call the extension method in the OnInitialized or OnInitializedAsync overriden methods for each component as following

```csharp

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

  

```csharp
// Without Source Generator
_language["Welcome", new

{

Username = "aksoftware98",

Version = "v4.0"

}]


// With Source Generator
KeysAccessor.Welcome("aksoftware98", "v4.0")
```

  

## Check the Sample Folder

Check the sample project here to see how to develop a full Blazor WebAssembly project with storing the last selected language with more than 8 languages available for one UI:

[Full Blazor WASM Sample](https://github.com/aksoftware98/multilanguages/tree/master/src/BlazorAKLocalization)

# Upcoming in Version 6.1
We are currently working on version 6.  Here are the upcoming features.
Verify All Keys Can Be Found](#verify-all-keys-can-be-found)
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

```csharp
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

```csharp
var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
var languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
var codeGeneratorService= new GenerateStaticKeysService(languageContainer);
string namespace = "MyCompany.Project";
string enumName = "LanguageKeys";
string filePath = @"c:\somedirectory\LanguageKeys.cs"
codeGeneratorService.CreateEnumKeysFile(namespaceName, enumName, filePath);
```

This will produce a file like this.

```csharp
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

```csharp
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

```csharp
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

```csharp
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
```csharp
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

## Create or Update Resource File from Localizable Strings
Instead of manually creating all the key value pairs, you can parse your .razor, HTML, etc files and create your en-US.yml file

Example
```C#
public void CreateOrUpdateResourceFileFromLocalizableStringsExample()
{
	ParseParms parms = new ParseParms();
	string solutionPath = TestHelper.GetSolutionPath();
	string componentsPath = Path.Combine(solutionPath, "BedBrigade.Client", "Components");
	string modelPath = Path.Combine(solutionPath, "BedBrigade.Common", "Models");
	parms.SourceDirectories = new List<string>() { componentsPath, modelPath };
	parms.WildcardPatterns = new List<string>() { "*.razor", "*.cs" };
	parms.ExcludeDirectories = new List<string>();
	parms.ExcludeFiles = new List<string>();
	parms.ResourceFilePath = Path.Combine(TestHelper.GetSolutionPath(), "BedBrigade.Client", "Resources", "en-US.yml");
	CreateCodeLogic logic = new CreateCodeLogic();
	logic.CreateOrUpdateResourceFileFromLocalizableStrings(parms);
}
```
## Replace Localizable Strings With Variables
Instead of manually searching and replacing all of the hard coded localizable strings in your UI code, you can automatically create a resource file and then replace them with the keys.

Example
```C#
public void ReplaceLocalizableStringsWithVariablesExample()
{
	ParseParms parms = new ParseParms();
	string solutionPath = TestHelper.GetSolutionPath();
	string componentsPath = Path.Combine(solutionPath, "BedBrigade.Client", "Components");
	string modelPath = Path.Combine(solutionPath, "BedBrigade.Common", "Models");
	parms.SourceDirectories = new List<string>() { componentsPath, modelPath };
	parms.WildcardPatterns = new List<string>() { "*.razor", "*.cs" };
	parms.ExcludeDirectories = new List<string>();
	parms.ExcludeFiles = new List<string>();
	parms.ResourceFilePath = Path.Combine(TestHelper.GetSolutionPath(), "BedBrigade.Client", "Resources", "en-US.yml");
	CreateCodeLogic logic = new CreateCodeLogic();
	logic.ReplaceLocalizableStringsWithVariables(parms);
}
```

# Thanks for the awesome contributors

<a  href="https://github.com/aksoftware98/multilanguages/graphs/contributors">

<img  src="https://contrib.rocks/image?repo=aksoftware98/multilanguages"  />

</a>
