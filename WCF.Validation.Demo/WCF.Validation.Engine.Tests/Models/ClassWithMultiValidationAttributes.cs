using System.ComponentModel.DataAnnotations;

namespace WCF.Validation.Engine.Tests.Models
{
    class ClassWithMultiValidationAttributes
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string Name { get; set; }

        [StringLength(6)]
        public string Company { get; set; }
    }
}
