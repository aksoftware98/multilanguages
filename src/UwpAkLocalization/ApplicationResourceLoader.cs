using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.UI.Xaml.Resources;
using AKSoftware.Localization.MultiLanguages;

namespace UwpAkLocalization
{
    public interface IResourceLoader
    {
        string GetResourceString(string resourceId);
    }

    public class ApplicationResourceLoader : CustomXamlResourceLoader, IResourceLoader
    {
        private ILanguageContainerService Localization { get;  }

        public ApplicationResourceLoader(ILanguageContainerService localization)
        {
            Localization = localization;
        }
        protected override object GetResource(string resourceId, string objectType, string propertyName, string propertyType)
        {
            return (object)Localization[resourceId];
        }

        public string GetResourceString(string resourceId)
        {
            throw new System.NotImplementedException();
        }
    }
}
