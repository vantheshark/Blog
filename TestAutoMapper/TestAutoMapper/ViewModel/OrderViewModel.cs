using System.Collections.Generic;
using AutoMapper;
using Autofac;
using TestAutoMapper.Converter;
using TestAutoMapper.Model;

namespace TestAutoMapper.ViewModel
{
    public class OrderViewModel
    {
        public List<OrderDetailsViewModel> Details { get; set; }
        public int Id { get; set; }
    }


    public class OrderMapping : IStartable
    {
        public void Start()
        {
            Mapper.CreateMap<Order, OrderViewModel>()
                  .ConvertUsing<OrderConverter>();
        }
    }
}
