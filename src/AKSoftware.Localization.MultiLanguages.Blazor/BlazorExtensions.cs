using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKSoftware.Localization.MultiLanguages.Blazor
{
    public static class BlazorExtensions
    {

        /// <summary>
        /// Track the state of the component, and it will be updated whenever the SetLanguage function has been called for the client
        /// </summary>
        /// <param name="language">Langauge Container</param>
        /// <param name="component">Component to be tracked</param>
        public static void InitLocalizedComponent(this ILanguageContainerService language, ComponentBase component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var extension = CreateExtension(component);
            language.AddExtension(extension);
        }

        /// <summary>
        /// Track the state of the component and return an IDisposable that removes the extension on dispose.
        /// Call this in OnInitialized and dispose the result in Dispose() for deterministic cleanup.
        /// </summary>
        /// <param name="language">Language Container</param>
        /// <param name="component">Component to be tracked</param>
        /// <returns>An IDisposable that removes the extension when disposed</returns>
        public static IDisposable InitLocalizedComponentWithDisposable(this ILanguageContainerService language, ComponentBase component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var extension = CreateExtension(component);
            language.AddExtension(extension);
            return new ExtensionDisposable(language, extension);
        }

        private static ComponentExtension CreateExtension(ComponentBase component)
        {
            var extension = new ComponentExtension(component);

            var type = typeof(ComponentBase);
            var stateHasChangedMethod = type.GetMethod("StateHasChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var dispatcherFunction = type.GetMethod("InvokeAsync",
                                                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                                                    null,
                                                    new Type[] { typeof(Action) },
                                                    null);

            var action = new Action<object>(e =>
            {
                if (e == null)
                    return;

                dispatcherFunction.Invoke(e, new[] { new Action(() =>
                {
                    stateHasChangedMethod.Invoke(e, null);
                }) });
            });

            extension.Action = action;
            return extension;
        }

    }

}
