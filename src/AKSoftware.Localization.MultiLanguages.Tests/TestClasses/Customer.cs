using System.ComponentModel.DataAnnotations;

namespace AKSoftware.Localization.MultiLanguages.Tests.TestClasses
{
    public class Customer
    {
        [Required]
        public string Name { get; set; }
    }
}
