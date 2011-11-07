using System;
using System.Collections.Generic;
using System.Linq;

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
            return IsRequired 
                   ? new[] {new RequiredOnNullPropertyValidator(this)} 
                   : Enumerable.Empty<ModelValidator>();
        }

        private class RequiredOnNullPropertyValidator : ModelValidator
        {
            public RequiredOnNullPropertyValidator(ModelMetadata metadata) : base(metadata)
            {
            }

            public override IEnumerable<ModelValidationResult> Validate(object container)
            {
                return new[] { new ModelValidationResult
                                   {
                                       Message = string.Format("{0} is required and cannot be null", Metadata.PropertyName),
                                       MemberName = Metadata.FullName ?? Metadata.PropertyName
                                   } };
            }
        }
    }
}
