# AKSoftware.Localization.MultiLanguages
[![Build Badge](https://aksoftware98.visualstudio.com/AkMultiLanguages/_apis/build/status/aksoftware98.multilanguages?branchName=master)](https://aksoftware98.visualstudio.com/AkMultiLanguages/_build/latest?definitionId=4&branchName=master)

![Nuget](https://img.shields.io/nuget/dt/AKSoftware.Localization.MultiLanguages?color=nuget&label=Nuget&style=plastic)

Build awesome Blazor WebAssembly web applications that supports more than 69+ languages with just a few lines of code, in addition to an easy translation tool that helps you translate all your content to any language you want with just one click

https://akmultilanguages.azurewebsites.net
Build with Love by Ahmad Mozaffar
http://ahmadmozaffar.net

## YouTube Session 
https://youtu.be/Xz68c8GBYz4

![Simple UI supports German](https://github.com/aksoftware98/multilanguages/blob/master/Example/BlazorWasmMultiLanguages/BlazorWasmMultiLanguages/wwwroot/German.png?raw=true)

![Blazor UI with Japanease](https://raw.githubusercontent.com/aksoftware98/multilanguages/master/Example/BlazorWasmMultiLanguages/BlazorWasmMultiLanguages/wwwroot/Japan.png)

# Getting Started

For Nuget Package Manager install the package
Install-Package AKSoftware.Localization.MultiLanguages 

## Create the Resources Folder

Inside your project create a folder called "Resources"
and inside it create a file called "en-US.yml" which is a YAML file 
then set your keywords inside the file like this 

    HelloWorld: Hello World
    Welcome: Welcome
    ...

> We chose YAML files because it's very light comparing it to XML or JSON and make the output dll very small, in addition to that it's much way faster in serialization and deserialization 

## Set the build action of the file to EmbeddedResource

Select the file in the Solution Explorer window and from the properties window set the build action property to "Embbed Resources"

## Translate the file

Visit the online translation tool on the following link 
https://akmultilanguages.azurewebsites.net

Go to translate app page

Upload your YAML file and click submit
All the languages will be available with just one click - install all the languages you want to support in your application 

## Import the files to the Resources folder

Import the files to the resources folder you have just created and set the build action property for them as Embedded Resources also 

## Coding time

Go to program.cs and register the Language Container Service in the Dependency Injection container
Import the library 
    using AKSoftware.Localization.MultiLanguages

Register the service 

    // Specify the assembly that has the langauges files, in this situation it's the current assembly 
    builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
	// You can specify the default culture of the project like this 
    // builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("fr-Fr"));

If you don't specify a default culture the library will try to find the file that matches the culture of the current user, if it's not existing it will try to find any file that matches the same language, then if it's not there it will try to find the English file then the first file in the folder, otherwise it will throw an exception 
# Start dealing with components 
With in your components that you want to localize inject the service 

    @inject ILanguageContainerService  languageContainer
And start getting the values from your files just like this 

    <h1>@languageContainer.Keys["HelloWorld"]</h1>

## Change the language from the UI

You are able to change the language and choose any language you have added from the UI like this 
Inject the service in the component 

        @inject ILanguageContainerService  languageContainer

Add a button and set the @onclick method

    <button   @onclick="SetFrench">French</button>
    @code
    {
	    void SetFrench()
	    {
		    languageContainer.SetLanguage(System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));
	    }
    }

## Check the Examples Folder
Check the examples folder which has a full example of how to build Blazor apps that supports about +7 languages

Developed with Love by Ahmad Mozaffar
http://ahmadmozaffar.net

