using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WCF.Validation.Engine
{
    public abstract class AssociatedValidatorProvider : ModelValidatorProvider
    {
        public override sealed IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata)
        {
            Check.Requires<ArgumentNullException>(metadata != null, "metadata");

            if (metadata.ContainerType != null && !String.IsNullOrEmpty(metadata.PropertyName))
            {
                return GetValidatorsForProperty(metadata);
            }

            return GetValidatorsForType(metadata);
        }

        protected abstract IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<Attribute> attributes);

        private IEnumerable<ModelValidator> GetValidatorsForProperty(ModelMetadata metadata)
        {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(metadata.ContainerType);
            
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(metadata.PropertyName, true);
            Check.Requires<ArgumentException>(property != null, string.Format("Property {0} not found in {1}", metadata.ContainerType.FullName, metadata.PropertyName));

            return GetValidators(metadata, property.Attributes.OfType<Attribute>());
        }

        private IEnumerable<ModelValidator> GetValidatorsForType(ModelMetadata metadata)
        {
            return GetValidators(metadata, GetTypeDescriptor(metadata.ModelType).GetAttributes().Cast<Attribute>());
        }

        protected virtual ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return TypeDescriptorHelper.Get(type);
        }
    }
}
