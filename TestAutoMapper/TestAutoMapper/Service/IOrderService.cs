using System.Collections.Generic;
using TestAutoMapper.Model;

namespace TestAutoMapper.Service
{
    public interface IOrderService
    {
        Order GetById(int id);
        IEnumerable<OrderDetails> GetOrderDetailsByOrderId(int orderId);
    }
}
