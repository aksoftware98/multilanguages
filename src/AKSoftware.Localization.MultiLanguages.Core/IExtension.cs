using System;
using System.Collections.Generic;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages
{
    public interface IExtension
    {

        object Component { get; set; }

        Action<object> Action { get; set; }

    }
}
