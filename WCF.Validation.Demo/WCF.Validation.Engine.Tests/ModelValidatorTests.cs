using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using WCF.Validation.Contract;
using WCF.Validation.Engine.Tests.Models;

// ReSharper disable InconsistentNaming
namespace WCF.Validation.Engine.Tests
{
    [TestFixture]
    public class ModelValidatorTests
    {
        [Test]
        public void Validate_can_validate_nested_item_in_list()
        {
            // Arrange
            var model = new ParentClass
            {
                Children = new List<NestedClass>
                {
                    new NestedClass
                    {
                        Name = "Van Thoai Nguyen"
                    }
                }
            };

            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current,
                                             null,
                                             () => model,
                                             typeof(ParentClass),
                                             null);


            var validator = ModelValidator.GetModelValidator(metaData);
            var result = validator.Validate(model).ToList();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("The field Name must be a string with a maximum length of 5.", result[0].Message);
            Assert.AreEqual("ParentClass.Children[0].Name", result[0].MemberName);
        }

        [Test]
        public void Validate_can_validate_on_null_but_required_complex_property()
        {
            // Arrange
            var model = new ParentClassWithARequiredProperty();

            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current,
                                             null,
                                             () => model,
                                             typeof(ParentClassWithARequiredProperty),
                                             null);


            var validator = ModelValidator.GetModelValidator(metaData);
            var result = validator.Validate(model).ToList();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("The IamRequired field is required.", result[0].Message);
            Assert.AreEqual("ParentClassWithARequiredProperty.IamRequired", result[0].MemberName);
        }

        [Test]
        public void Validate_can_validate_stringlength_rule()
        {
            // Arrange
            var model = new NestedClass
            {
                Name = "Van Thoai Nguyen"
            };

            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current,
                                             null,
                                             () => model,
                                             typeof(NestedClass),
                                             null);


            var validator = ModelValidator.GetModelValidator(metaData);
            var result = validator.Validate(model).ToList();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("NestedClass.Name", result[0].MemberName);
            Assert.AreEqual("The field Name must be a string with a maximum length of 5.", result[0].Message);
        }

        [Test]
        public void Validate_does_not_validate_require_rule_by_default()
        {
            // Arrange
            var model = new NestedClass
            {
                Name = null
            };

            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current,
                                             null,
                                             () => model,
                                             typeof(NestedClass),
                                             null);


            var validator = ModelValidator.GetModelValidator(metaData);
            var result = validator.Validate(model).ToList();

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void Validate_can_validate_all_attributes()
        {
            // Arrange
            var model = new ClassWithMultiValidationAttributes
                            {
                                Company = "My Company Name"
                            };

            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current,
                                             null,
                                             () => model,
                                             typeof(ClassWithMultiValidationAttributes),
                                             null);


            var validator = ModelValidator.GetModelValidator(metaData);
            var result = validator.Validate(model).ToList();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("The Name field is required.", result[0].Message);
            Assert.AreEqual("ClassWithMultiValidationAttributes.Name", result[0].MemberName);
            Assert.AreEqual("The field Company must be a string with a maximum length of 6.", result[1].Message);
            Assert.AreEqual("ClassWithMultiValidationAttributes.Company", result[1].MemberName);
        }

        [Test]
        public void Validate_on_basic_types()
        {
            // Arrange
            var model = "Thoai";
            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());

            var validator = ModelValidator.GetModelValidator(metadata);

            // Action
            var result = validator.Validate(model);


            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void Test_complex_validation()
        {
            // Arrange
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


            // Action
            var validator = ModelValidator.GetModelValidator(metadata);
            var validationResults = validator.Validate(null).ToList();

            // Assert
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreEqual("Order.Details[0].ProductName", validationResults[0].MemberName);
            Assert.AreEqual("The field ProductName must be a string with a maximum length of 5.", validationResults[0].Message);

            Assert.AreEqual("Order.FirstOrderDetail.ProductName", validationResults[1].MemberName);
            Assert.AreEqual("The field ProductName must be a string with a maximum length of 5.", validationResults[1].Message);
        }
    }
}
// ReSharper restore InconsistentNaming