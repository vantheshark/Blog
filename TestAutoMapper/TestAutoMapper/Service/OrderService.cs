using System;
using System.Collections.Generic;
using TestAutoMapper.Model;

namespace TestAutoMapper.Service
{
    public class OrderService : IOrderService
    {
        public Order GetById(int id)
        {
            return new Order
            {
                Id = id, 
                Details = new List<OrderDetails>()
            };
        }

        public IEnumerable<OrderDetails> GetOrderDetailsByOrderId(int orderId)
        {
            Console.WriteLine("GetOrderDetailsByOrderId is called");
            return new List<OrderDetails>
            {
                new OrderDetails
                {
                    Id = 1,
                    OrderId = orderId,
                    Price = 10,
                    ProductId = 1
                },
                new OrderDetails
                {
                    Id = 2,
                    OrderId = orderId,
                    Price = 20,
                    ProductId = 2
                },
                new OrderDetails
                {
                    Id = 3,
                    OrderId = orderId,
                    Price = 30,
                    ProductId = 3
                },
            };
        }
    }
}