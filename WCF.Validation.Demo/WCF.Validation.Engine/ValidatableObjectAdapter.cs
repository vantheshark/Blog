using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace WCF.Validation.Engine
{
    public class ValidatableObjectAdapter : ModelValidator
    {
        public ValidatableObjectAdapter(ModelMetadata metadata)
            : base(metadata)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // NOTE: Container is never used here, because IValidatableObject doesn't give you
            // any way to get access to your container.

            object model = Metadata.ModelValue;
            if (model == null)
            {
                return Enumerable.Empty<ModelValidationResult>();
            }

            var validatable = model as IValidatableObject;
            if (validatable == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "ValidatableObjectAdapter_IncompatibleType {0} {1}",
                        typeof(IValidatableObject).FullName,
                        model.GetType().FullName
                    )
                );
            }

            var validationContext = new ValidationContext(validatable, null, null);
            return ConvertResults(validatable.Validate(validationContext));
        }

        private IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> results)
        {
            foreach (ValidationResult result in results)
            {
                if (result != ValidationResult.Success)
                {
                    if (result.MemberNames == null || !result.MemberNames.Any())
                    {
                        yield return new ModelValidationResult { Message = result.ErrorMessage };
                    }
                    else
                    {
                        foreach (string memberName in result.MemberNames)
                        {
                            yield return new ModelValidationResult { Message = result.ErrorMessage, MemberName = memberName };
                        }
                    }
                }
            }
        }
    }
}
