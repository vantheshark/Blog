using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestAutoMapper.Model;
using TestAutoMapper.Service;
using TestAutoMapper.ViewModel;

namespace TestAutoMapper.Converter
{
    public class OrderConverter : ITypeConverter<Order, OrderViewModel>
    {
        private readonly IOrderService _orderService;

        public OrderConverter(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public OrderViewModel Convert(ResolutionContext context)
        {
            var order = context.SourceValue as Order;
            if (order == null)
            {
                return null;
            }

            var orderDetails = _orderService.GetOrderDetailsByOrderId(order.Id);
            return new OrderViewModel
            {
                Id = order.Id,
                Details = Mapper.Map<IEnumerable<OrderDetails>, IEnumerable<OrderDetailsViewModel>>(orderDetails).ToList()
            };
        }
    }
}
