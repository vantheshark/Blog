
using System.ComponentModel.DataAnnotations;

namespace WCF.Validation.Engine.Tests.Models
{
    class NestedClass
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Name { get; set; }
    }
}
