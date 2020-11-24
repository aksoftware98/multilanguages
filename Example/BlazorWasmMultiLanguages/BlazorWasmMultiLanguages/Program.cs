using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

namespace BlazorWasmMultiLanguages
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            // Load the language container with files from the Resources Folder
            builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());

            // Load the language Container with the files from a custom folder
            //builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly(), folderName: "TestFolder");

            builder.Services.AddScoped<HttpClient>();

            // Launch the app with a default culture 
            //builder.Services.AddLangaugeContainer(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("fr-Fr"));
            
            await builder.Build().RunAsync();
        }
    }
}
