
namespace TestAutoMapper.Model
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
}
