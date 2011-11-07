using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WCF.Validation.Contract;

// ReSharper disable InconsistentNaming
namespace WCF.Validation.Engine.Tests
{
    [TestFixture]
    public class ModelMetataProviderTests
    {
        [Test]
        public void Validate_on_basic_types()

        {
            var model = "Thoai";
            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());

            var validator = ModelValidator.GetModelValidator(metadata);

            var result = validator.Validate(model);

            Assert.AreEqual(0, result.Count());

        }

        [Test]
        public void Test_complex_validation()
        {
            var order = new Order
            {
                OrderId = 1,
                FirstOrderDetail = new OrderDetail
                                       {
                                           OrderId = 1,
                                           Price = 100,
                                           ProductName = "P123456"
                                       },
                Details = new List<OrderDetail>
                                              {
                                                  new OrderDetail
                                                      {
                                                          OrderId = 1,
                                                          Price = 100,
                                                          ProductName = "P123456"
                                                      },
                                                  new OrderDetail
                                                      {
                                                          OrderId = 1,
                                                          Price = 200,
                                                          ProductName = "P2"
                                                      }
                                              }


            };
            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => order, order.GetType());

            var validator = ModelValidator.GetModelValidator(metadata);
            var validationResults = validator.Validate(null).ToList();
            
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreEqual("Order.Details[0].ProductName", validationResults[0].MemberName);
            Assert.AreEqual("The field ProductName must be a string with a maximum length of 5.", validationResults[0].Message);

            Assert.AreEqual("Order.FirstOrderDetail.ProductName", validationResults[1].MemberName);
            Assert.AreEqual("The field ProductName must be a string with a maximum length of 5.", validationResults[1].Message);
        }
    }
}
// ReSharper restore InconsistentNaming