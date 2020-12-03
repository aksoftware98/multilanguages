using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

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

            // Init the extension 
            var extension = new ComponentExtension()
            {
                Component = component,
            };
            
            var action = new Action<object>(e =>
            {
                // Call the StateHasChanged function for the component 
                var type = typeof(ComponentBase);
                var stateHasChangedMethod = type.GetMethod("StateHasChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                stateHasChangedMethod.Invoke(extension.Component, null); 
            });

            extension.Action = action;

            language.AddExtension(extension);
        }

    }
}
