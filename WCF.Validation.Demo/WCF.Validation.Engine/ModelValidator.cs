using System;
using System.Collections.Generic;

namespace WCF.Validation.Engine
{
    public abstract class ModelValidator
    {
        protected ModelValidator(ModelMetadata metadata)
        {
            Check.Requires<ArgumentNullException>(metadata != null);
            Metadata = metadata;
        }

        public virtual bool IsRequired
        {
            get
            {
                return false;
            }
        }

        protected internal ModelMetadata Metadata { get; private set; }

        public static ModelValidator GetModelValidator(ModelMetadata metadata)
        {
            return new CompositeModelValidator(metadata);
        }

        public abstract IEnumerable<ModelValidationResult> Validate(object container);

        private class CompositeModelValidator : ModelValidator
        {
            public CompositeModelValidator(ModelMetadata metadata)
                : base(metadata)
            {
            }

            public override IEnumerable<ModelValidationResult> Validate(object container)
            {
                foreach (ModelMetadata propertyMetadata in Metadata.Properties)
                {
                    foreach (ModelValidator propertyValidator in propertyMetadata.GetValidators())
                    {
                        foreach (ModelValidationResult propertyResult in propertyValidator.Validate(Metadata.ModelValue))
                        {
                            yield return new ModelValidationResult
                            {
                                MemberName = propertyResult.MemberName ?? propertyMetadata.FullName,
                                Message = propertyResult.Message
                            };
                        }
                    }
                }
            }
        }
    }
}
