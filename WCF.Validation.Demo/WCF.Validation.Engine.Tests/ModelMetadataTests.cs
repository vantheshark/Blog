using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;
using WCF.Validation.Contract;
using WCF.Validation.Engine.Tests.Models;

// ReSharper disable InconsistentNaming
namespace WCF.Validation.Engine.Tests
{
    [TestFixture]
    public class ModelMetadataTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [TestCase("string", typeof(string))]
        [TestCase('1', typeof(char))]
        [TestCase(1, typeof(sbyte))]
        [TestCase(1, typeof(byte))]
        [TestCase(1, typeof(short))]
        [TestCase(1, typeof(ushort))]
        [TestCase(1, typeof(int))]
        [TestCase(1, typeof(uint))]
        [TestCase(1, typeof(long))]
        [TestCase(1, typeof(ulong))]
        [TestCase(1.1, typeof(float))]
        [TestCase(1.1, typeof(double))]
        [TestCase(1.1, typeof(decimal))]

        public void IsComplexType_should_return_false_if_type_is_builtint_simple_type(object model, Type modelValue)
        {
            Assert.IsFalse(ModelMetadataProviders.Current.GetMetadataForType(() => model, modelValue).IsComplexType);
        }

        [TestCase(typeof(Order))]
        [TestCase(typeof(OrderDetail))]
        [TestCase(typeof(List))]
        [TestCase(typeof(StringBuilder))]
        public void IsComplexType_should_return_true_if_type_is_complext_type(Type modelValue)
        {
            Assert.IsTrue(ModelMetadataProviders.Current.GetMetadataForType(() => null, modelValue).IsComplexType);
        }

        [Test]
        public void Properties_should_return_a_single_metadata_for_a_enumerable_property()
        {
            // Arrange
            var model = new ParentClass
            {
                Children = new List<NestedClass>
                {
                    new NestedClass(),
                    new NestedClass()
                }
            };

            // Action
            var properties = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(ParentClass)).Properties.ToList();

            // Assert
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual("ParentClass.Children", properties[0].FullName);
        }

        [Test]
        public void GetValidators_should_return_validators_for_item_in_list_and_validators_for_itself()
        {
            // Arrange
            var model = new ParentClass
            {
                Children = new List<NestedClass>
                {
                    new NestedClass(),
                    new NestedClass()
                }
            };

            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current, 
                                             typeof(ParentClass), 
                                             () => model.Children, 
                                             typeof(List<NestedClass>), 
                                             "Children");


            var validators = metaData.GetValidators().ToList();

            // Assert
            Assert.AreEqual(3, validators.Count());
        }

        [Test]
        public void ModelValue_set_should_reset_properties()
        {
            // Arrange
            var model = new NestedClass
                            {
                                Id = 1,
                                Name = "Van Thoai Nguyen"
                            };
            // Action
            var metaData = new ModelMetadata(ModelMetadataProviders.Current,
                                             null,
                                             () => model,
                                             typeof(NestedClass),
                                             null);

            // Assert
            Assert.AreEqual(1, metaData.Properties.ToList()[0].ModelValue);
            metaData.ModelValue = new NestedClass
            {
                Id = 2,
                Name = "Van Thoai Nguyen 2"
            };
            Assert.AreEqual(2, metaData.Properties.ToList()[0].ModelValue);
        }
    }
}
// ReSharper restore InconsistentNaming