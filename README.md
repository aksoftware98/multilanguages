
  
  

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

**The ability to get all the keys for the current culture.**

```C#
List<string> keys = _language.GetKeys();
```

**The ability to loop through the key values for the current culture.**

```C#
foreach (KeyValuePair<object, object> keyValue in _service.Keys)
{
	Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
}
```

**The ability to get all the registered languages.**

```C#
List<CultureInfo> registeredLanguages = _language.RegisteredLanguages;
```

Thanks for the awesome contributors

<a  href="https://github.com/aksoftware98/multilanguages/graphs/contributors">

<img  src="https://contrib.rocks/image?repo=aksoftware98/multilanguages"  />

</a>
