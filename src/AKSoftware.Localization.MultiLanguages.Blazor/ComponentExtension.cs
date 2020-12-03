using AKSoftware.Localization.MultiLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKSoftware.Localization.MultiLanguages.Blazor
{
    public class ComponentExtension : IExtension
    {
        public object Component { get; set; }
        public Action<object> Action { get; set; }
    }
}
