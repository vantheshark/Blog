using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCF.Validation.Contract
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public List<OrderDetail> Details { get; set; }

        [DataMember]
        public OrderDetail FirstOrderDetail { get; set; }
    }
}
