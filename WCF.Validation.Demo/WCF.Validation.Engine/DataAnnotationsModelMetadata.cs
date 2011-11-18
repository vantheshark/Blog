using System;
using System.Collections.Generic;

namespace WCF.Validation.Engine
{
    public class DataAnnotationsModelMetadata : ModelMetadata
    {
        public DataAnnotationsModelMetadata(DataAnnotationsModelMetadataProvider provider, Type containerType,
                                            Func<object> modelAccessor, Type modelType, string propertyName)
            : base(provider, containerType, modelAccessor, modelType, propertyName)
        {
        }

        protected override IEnumerable<ModelValidator> GetValidatorForNullModel()
        {
            return ModelValidatorProviders.Providers.GetValidators(this); // Must return validators for other attributes decorate on the property even though it's null
        }
    }
}
