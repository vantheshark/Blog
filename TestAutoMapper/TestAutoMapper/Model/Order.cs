using System.Collections.Generic;

namespace TestAutoMapper.Model
{
    public class Order
    {
        public List<OrderDetails> Details { get; set; }
        public int Id { get; set; }
    }
}
