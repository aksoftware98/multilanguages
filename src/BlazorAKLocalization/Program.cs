using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AKSoftware.Localization.MultiLanguages;

namespace BlazorAKLocalization
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Load the language container with files from the Resources Folder
            builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());

            // Load the language Container with the files from a custom folder
            //builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly(), folderName: "TestFolder");

            builder.Services.AddScoped<HttpClient>();


            await builder.Build().RunAsync();
        }
    }
}
