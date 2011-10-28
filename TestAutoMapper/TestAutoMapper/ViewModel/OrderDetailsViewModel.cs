
using AutoMapper;
using Autofac;
using TestAutoMapper.Model;

namespace TestAutoMapper.ViewModel
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }

    public class OrderDetailsViewModelMapping : IStartable
    {
        public void Start()
        {
            Mapper.CreateMap<OrderDetails, OrderDetailsViewModel>();
        }
    }
}
