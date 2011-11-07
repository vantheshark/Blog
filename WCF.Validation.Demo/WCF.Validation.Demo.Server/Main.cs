using System;
using System.ServiceModel;
using WCF.Validation.Contract;
using WCF.Validation.Engine;

namespace WCF.Validation.Demo.Server
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            // Configure and open the local gateway
            ServiceHost host2 = new ServiceHost(typeof(OrderServiceImplementation));
            host2.Open();

            ColorConsole.WriteLine(ConsoleColor.Green, "OrderService is running.");

            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();
            host2.Close();
        }
    }
}
