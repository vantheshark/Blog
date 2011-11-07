using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WCF.Validation.Engine
{
    public class DataAnnotationsModelValidator : ModelValidator
    {
        public DataAnnotationsModelValidator(ModelMetadata metadata,ValidationAttribute attribute)
            : base(metadata)
        {

            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            Attribute = attribute;
        }

        protected internal ValidationAttribute Attribute { get; private set; }

        protected internal string ErrorMessage
        {
            get
            {
                return Attribute.FormatErrorMessage(Metadata.FullName);
            }
        }

        public override bool IsRequired
        {
            get
            {
                return Attribute is RequiredAttribute;
            }
        }

        internal static ModelValidator Create(ModelMetadata metadata, ValidationAttribute attribute)
        {
            return new DataAnnotationsModelValidator(metadata, attribute);
        }

       

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // Per the WCF RIA Services team, instance can never be null (if you have
            // no parent, you pass yourself for the "instance" parameter).
            ValidationContext context = new ValidationContext(container ?? Metadata.ModelValue, null, null);
            context.DisplayName = Metadata.PropertyName ?? Metadata.FullName;

            ValidationResult result = Attribute.GetValidationResult(Metadata.ModelValue, context);
            if (result != ValidationResult.Success)
            {
                yield return new ModelValidationResult
                {
                    Message = result.ErrorMessage
                };
            }
        }
    }
}
