using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized with a specific culture
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="culture">Initial culture</param>
        /// <returns></returns>
        public static IServiceCollection AddLangaugeContainer(this IServiceCollection services, Assembly assembly, CultureInfo culture)
        {
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s => new LanguageContainerInAssembly(assembly, culture));
        }

        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized with the user culture
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly">Assembly that contains the Resource folder which has the language files</param>
        /// <returns></returns>
        public static IServiceCollection AddLangaugeContainer(this IServiceCollection services, Assembly assembly)
        {
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s => new LanguageContainerInAssembly(assembly));
        }


        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized from the executing assembly
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <returns></returns>
        //public static IServiceCollection AddLangaugeContainer(this IServiceCollection services)
        //{
        //    return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s => new LanguageContainerInAssembly(Assembly.GetEntryAssembly()));
        //}

        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized from the executing assembly
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="defaultCulture">Default Culture</param>
        /// <returns></returns>
        //public static IServiceCollection AddLangaugeContainer(this IServiceCollection services, CultureInfo defaultCulture)
        //{
        //    return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s => new LanguageContainerInAssembly(Assembly.GetEntryAssembly(), defaultCulture));
        //}

    }
}
