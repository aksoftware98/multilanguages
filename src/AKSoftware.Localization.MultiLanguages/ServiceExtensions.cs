using System;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using AKSoftware.Localization.MultiLanguages.Providers;

namespace AKSoftware.Localization.MultiLanguages
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Register a singleton instance of LanguageContainer class initialized with a specific culture
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly"></param>
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
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainer<TKeysProvider>(this IServiceCollection services, Assembly assembly, string folderName = "Resources")
        where TKeysProvider : IKeysProvider
        {
            services.AddSingleton<IKeysProvider>(s => (TKeysProvider)Activator.CreateInstance(typeof(TKeysProvider), assembly, folderName));
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }

        public static IServiceCollection AddLanguageContainer<TKeysProvider>(this IServiceCollection services, Assembly assembly, LocalizationFolderType localizationFolderType = LocalizationFolderType.InstallationFolder, string folderName = "Resources")
            where TKeysProvider : IKeysProvider
        {
            services.AddSingleton<IKeysProvider>(s => (TKeysProvider)Activator.CreateInstance(typeof(TKeysProvider), assembly, folderName, localizationFolderType));
            return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }

		/// <summary>
		/// Add a language container that loads the language files from a specific folder
		/// </summary>
		/// <param name="services"></param>
		/// <param name="folderPath">Path of the folder that contains the YAML files of the language</param>
		/// <param name="defaultCulture">Default culture you want the app to start with</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection AddLanguageContainerFromFolder(this IServiceCollection services, string folderPath, CultureInfo defaultCulture)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentNullException(nameof(folderPath));
            if (defaultCulture == null)
                throw new ArgumentNullException(nameof(defaultCulture));
			
			return services.AddSingleton<ILanguageContainerService>(s =>
			{
				return new LanguageContainer(defaultCulture, new FolderResourceKeysProvider(folderPath));
			});
		}


        #region Blazor Server 
        /// <summary>
        /// Register a scoped instance of LanguageContainer class initialized with a specific culture
        /// Scoped instance helps the Blazor Server to serve many clients with different languages at the same time
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly"></param>
        /// <param name="culture">Initial culture</param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainerForBlazorServer(this IServiceCollection services, Assembly assembly, CultureInfo culture, string folderName = "Resources")
        {
            services.AddSingleton<IKeysProvider, EmbeddedResourceKeysProvider>(s =>
                new EmbeddedResourceKeysProvider(assembly, folderName));
            return services.AddScoped<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(culture, keysProvider);
            });
        }

        /// <summary>
        /// Register a scoped instance of LanguageContainer class initialized with a specific culture
        /// Scoped instance helps the Blazor Server to serve many clients with different languages at the same time
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly">Assembly that contains the Resource folder which has the language files</param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainerForBlazorServer(this IServiceCollection services, Assembly assembly, string folderName = "Resources")
        {
            services.AddSingleton<IKeysProvider, EmbeddedResourceKeysProvider>(s => new EmbeddedResourceKeysProvider(assembly, folderName));
            return services.AddScoped<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }

        /// <summary>
        /// Register a scoped instance of LanguageContainer class initialized with a specific culture
        /// Scoped instance helps the Blazor Server to serve many clients with different languages at the same time
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly">Assembly that contains the Resource folder which has the language files</param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainerForBlazorServer<TKeysProvider>(this IServiceCollection services, Assembly assembly, string folderName = "Resources")
        where TKeysProvider : IKeysProvider
        {
            services.AddSingleton<IKeysProvider>(s => (TKeysProvider)Activator.CreateInstance(typeof(TKeysProvider), assembly, folderName));
            return services.AddScoped<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }

        /// <summary>
        /// Register a scoped instance of LanguageContainer class initialized with a specific culture
        /// Scoped instance helps the Blazor Server to serve many clients with different languages at the same time
        /// </summary>
        /// <param name="services">Dependency Services provider</param>
        /// <param name="assembly">Assembly that contains the Resource folder which has the language files</param>
        /// <param name="localizationFolderType">Define the type of the localization folder</param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static IServiceCollection AddLanguageContainerForBlazorServer<TKeysProvider>(this IServiceCollection services, Assembly assembly, LocalizationFolderType localizationFolderType = LocalizationFolderType.InstallationFolder, string folderName = "Resources")
            where TKeysProvider : IKeysProvider
		{
            services.AddSingleton<IKeysProvider>(s => (TKeysProvider)Activator.CreateInstance(typeof(TKeysProvider), assembly, folderName, localizationFolderType));
            return services.AddScoped<ILanguageContainerService, LanguageContainerInAssembly>(s =>
            {
                var keysProvider = s.GetService<IKeysProvider>();
                return new LanguageContainerInAssembly(keysProvider);
            });
        }


		/// <summary>
		/// Add a language container that loads the language files from a specific folder
		/// </summary>
		/// <param name="services"></param>
		/// <param name="folderPath">Path of the folder that contains the YAML files of the language</param>
		/// <param name="defaultCulture">Default culture you want the app to start with</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection AddLanguageContainerFromFolderForBlazorServer(this IServiceCollection services, string folderPath, CultureInfo defaultCulture)
		{
			if (string.IsNullOrWhiteSpace(folderPath))
				throw new ArgumentNullException(nameof(folderPath));
			if (defaultCulture == null)
				throw new ArgumentNullException(nameof(defaultCulture));

			return services.AddScoped<ILanguageContainerService>(s =>
			{
				return new LanguageContainer(defaultCulture, new FolderResourceKeysProvider(folderPath));
			});
		}

		#endregion
	}
}
