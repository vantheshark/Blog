using System;
using WCF.Validation.Contract;
using WCF.Validation.Contract.Validators;

namespace WCF.Validation.Demo.Server
{
    public class OrderServiceImplementation : IOrderService
    {
        [ParameterValidator]
        public void CreateOrder(Order order)
        {
            ColorConsole.WriteLine(ConsoleColor.Green, "CreateOrder was called succesfully.");
        }
    }
}
