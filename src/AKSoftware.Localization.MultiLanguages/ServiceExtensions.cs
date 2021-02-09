using System;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

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
        public static IServiceCollection AddLanguageContainer(this IServiceCollection services, Assembly assembly, CultureInfo culture, string folderName = "Resources")
        {
            services.AddSingleton<IKeysProvider, EmbeddedResourceKeysProvider>(s =>
                new EmbeddedResourceKeysProvider(assembly, folderName));
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly( culture, keysProvider);
            });
        }

        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized with the user culture
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly">Assembly that contains the Resource folder which has the language files</param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainer(this IServiceCollection services, Assembly assembly, string folderName = "Resources")
        {
            services.AddSingleton<IKeysProvider, EmbeddedResourceKeysProvider>(s => new EmbeddedResourceKeysProvider(assembly, folderName));
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }

        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized with the user culture
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly">Assembly that contains the Resource folder which has the language files</param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainer<TKeysProvider>(this IServiceCollection services, Assembly assembly, string folderName = "Resources")
        where TKeysProvider : KeysProvider
        {
            services.AddSingleton<IKeysProvider, TKeysProvider>(s => (TKeysProvider)Activator.CreateInstance(typeof(TKeysProvider), assembly, folderName));
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }

        public static IServiceCollection AddLanguageContainer<TKeysProvider>(this IServiceCollection services, Assembly assembly, LocalizationFolderType localizationFolderType = LocalizationFolderType.InstallationFolder, string folderName = "Resources")
            where TKeysProvider : KeysProvider
        {
            services.AddSingleton<IKeysProvider, TKeysProvider>(s => (TKeysProvider)Activator.CreateInstance(typeof(TKeysProvider), assembly, folderName, localizationFolderType));
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }
    }
}
