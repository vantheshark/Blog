using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WCF.Validation.Contract
{
    [DataContract]
    public class Order
    {
        [DataMember]
        [Required]
        public int OrderId { get; set; }

        [DataMember]
        public List<OrderDetail> Details { get; set; }

        [DataMember]
        public OrderDetail FirstOrderDetail { get; set; }
    }
}
