using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WCF.Validation.Engine.Tests.Models
{
    class ParentClass
    {
        public List<NestedClass> Children { get; set; }
    }

    class ParentClassWithARequiredProperty
    {
        public List<NestedClass> Children { get; set; }

        [Required]
        public NestedClass IamRequired { get; set; }
    }
}
