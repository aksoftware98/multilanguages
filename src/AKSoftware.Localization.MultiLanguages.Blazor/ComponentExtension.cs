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
        private readonly WeakReference<object> _componentRef;

        public ComponentExtension(object component)
        {
            _componentRef = new WeakReference<object>(component);
        }

        public object Component
        {
            get
            {
                if (_componentRef.TryGetTarget(out var target))
                    return target;
                return null;
            }
            set
            {
                _componentRef.SetTarget(value);
            }
        }

        public Action<object> Action { get; set; }
    }
}
