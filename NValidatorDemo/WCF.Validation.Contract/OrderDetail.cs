using System.Runtime.Serialization;

namespace WCF.Validation.Contract
{
    [DataContract]
    public class OrderDetail
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public decimal Price { get; set; }
        
        [DataMember]
        public string ProductName { get; set; }
    }
}
