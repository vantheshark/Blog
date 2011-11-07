using System;
using System.Collections.Generic;
using System.ServiceModel;
using WCF.Validation.Contract;
using WCF.Validation.Engine;

namespace WCF.Validation.Demo.Client
{
    class Client
    {
        static void Main()
        {
            // Create a client with given client endpoint configuration
            try
            {
                var channelFactory = new ChannelFactory<IOrderService>("OrderService");
                var c = channelFactory.CreateChannel();
                c.CreateOrder(new Order
                                  {
                                      OrderId = 1,
                                      FirstOrderDetail = new OrderDetail
                                                             {
                                                                 OrderId = 1,
                                                                 Price = 100,
                                                                 ProductName = "P800056"
                                                             },
                                      Details = new List<OrderDetail>
                                                    {
                                                        new OrderDetail
                                                             {
                                                                 OrderId = 1,
                                                                 Price = 100,
                                                                 ProductName = "P800056"
                                                             },
                                                        new OrderDetail
                                                        {
                                                            OrderId = 1,
                                                            Price = 200,
                                                            ProductName = "P2"
                                                        }
                                                    }
                                                                

                                  });
            }
            catch (FaultException<ValidationFault> validationFault)
            {
                ColorConsole.WriteLine(ConsoleColor.Yellow, "{0}: {1}", validationFault.GetType(), validationFault.Message);
                ColorConsole.WriteLine(ConsoleColor.Red, validationFault.Detail.ErrorMessage);
            }
            catch (FaultException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType(), e.Message);
            }

            //Closing the client gracefully closes the connection and cleans up resources
            

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to terminate client.");
            Console.ReadLine();
        }
    }
}
