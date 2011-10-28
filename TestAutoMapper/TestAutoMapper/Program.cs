using System;
using AutoMapper;
using TestAutoMapper.Model;
using TestAutoMapper.ViewModel;

namespace TestAutoMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            BootStrapper.Run();

            var order = new Order {Id = 1};

            try
            {
                var viewModel = Mapper.Map<Order, OrderViewModel>(order);

                Console.WriteLine(string.Format("\nThere are {0} order details", viewModel.Details.Count));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
