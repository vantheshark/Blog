using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WCF.Validation.Engine
{
    public abstract class AssociatedMetadataProvider : ModelMetadataProvider
    {
        protected abstract ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, 
                                                        Type containerType, 
                                                        Func<object> modelAccessor, 
                                                        Type modelType, 
                                                        string propertyName);

        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            Check.Requires<ArgumentNullException>(containerType != null);
            return GetMetadataForPropertiesImpl(container, containerType);
        }

        private IEnumerable<ModelMetadata> GetMetadataForPropertiesImpl(object container, Type containerType)
        {
            foreach (PropertyDescriptor property in GetPropertyDescriptors(containerType))
            {
                Func<object> modelAccessor = container == null ? null : GetPropertyValueAccessor(container, property);
                yield return GetMetadataForProperty(modelAccessor, containerType, property);
            }
        }

        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            Check.Requires<ArgumentNullException>(containerType != null);
            Check.Requires<ArgumentException>(!string.IsNullOrEmpty(propertyName));

            var propertyDescriptors = GetPropertyDescriptors(containerType);
            PropertyDescriptor property = propertyDescriptors.ToList().FirstOrDefault(x => x.Name == propertyName);
            Check.Requires<ArgumentException>(property != null, String.Format("Property {1} not found in {0}", containerType.FullName, propertyName));

            return GetMetadataForProperty(modelAccessor, containerType, property);
        }

        protected virtual ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, PropertyDescriptor propertyDescriptor)
        {
            IEnumerable<Attribute> attributes = propertyDescriptor.Attributes.Cast<Attribute>();
            ModelMetadata result = CreateMetadata(attributes, containerType, modelAccessor, propertyDescriptor.PropertyType, propertyDescriptor.Name);
            return result;
        }

        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            Check.Requires<ArgumentNullException>(modelType != null);

            ICollection<Attribute> a = new List<Attribute>();
            GetPropertyDescriptors(modelType).ToList().ForEach(x => a.Union(x.Attributes.Cast<Attribute>()));

            var attributes = a.AsEnumerable();
            
            ModelMetadata result = CreateMetadata(attributes, null /* containerType */, modelAccessor, modelType, null /* propertyName */);
            return result;
        }

        private static Func<object> GetPropertyValueAccessor(object container, PropertyDescriptor property)
        {
            return () => property.GetValue(container);
        }

        protected virtual IEnumerable<PropertyDescriptor> GetPropertyDescriptors(Type type)
        {
            return TypeDescriptorHelper.GetPropertyDescriptors(type);
        }
    }
}
