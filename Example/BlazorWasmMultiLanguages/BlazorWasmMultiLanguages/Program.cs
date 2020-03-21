using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;

namespace BlazorWasmMultiLanguages
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddLangaugeContainer(Assembly.GetExecutingAssembly());
            
            await builder.Build().RunAsync();
        }
    }
}
