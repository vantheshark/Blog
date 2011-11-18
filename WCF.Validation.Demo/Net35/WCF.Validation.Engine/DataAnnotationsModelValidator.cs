using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // Per the WCF RIA Services team, instance can never be null (if you have
            // no parent, you pass yourself for the "instance" parameter).
            var context = new ValidationContext(container ?? Metadata.ModelValue, null, null)
                              {
                                  MemberName = Metadata.PropertyName,
                                  DisplayName = Metadata.PropertyName ?? Metadata.FullName
                              };

            ValidationResult result = Attribute.GetValidationResult(Metadata.ModelValue, context);
            if (result != ValidationResult.Success)
            {
                yield return GetModelValidationResult(result);
            }
        }

        protected virtual ModelValidationResult GetModelValidationResult(ValidationResult result)
        {
            return new ModelValidationResult
            {
                Message = result.ErrorMessage,
                MemberName = Metadata.FullName + (string.IsNullOrEmpty(Metadata.PropertyName) && result.MemberNames != null && result.MemberNames.Count() > 0 ? "." + result.MemberNames.FirstOrDefault() : string.Empty)
            };
        }
    }
}
