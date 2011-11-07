using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WCF.Validation.Contract
{
    [DataContract]
    public class OrderDetail
    {
        [DataMember]
        [Required]
        public int OrderId { get; set; }

        [DataMember]
        [Required]
        public decimal Price { get; set; }

        [StringLength(5)]
        [DataMember]
        public string ProductName { get; set; }
    }
}
