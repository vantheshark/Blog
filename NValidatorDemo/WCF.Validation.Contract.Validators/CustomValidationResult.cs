using System.Collections.Generic;
using NValidator;

namespace WCF.Validation.Contract.Validators
{
    /// <summary>
    /// This class is an implementation of ValidationResult that provides the information of ValidatedObject
    /// </summary>
    public class CustomValidationResult : FormattableMessageResult
    {
        public CustomValidationResult()
            : base(new Dictionary<string, object>())
        {
        }

        public CustomValidationResult(ValidationResult originalResult)
            : base(originalResult is FormattableMessageResult
                    ? (originalResult as FormattableMessageResult).Params
                    : new Dictionary<string, object>())
        {
            CopyValues(originalResult);
        }

        private void CopyValues(ValidationResult originalResult)
        {
            var customValidationResult = originalResult as CustomValidationResult;
            MemberName = originalResult.MemberName;
            Message = originalResult.Message;
            PropertyName = originalResult.PropertyName;

            if (customValidationResult != null && customValidationResult.ValidatedValueWasSet)
            {
                ValidatedValue = customValidationResult.ValidatedValue;
            }
        }

        public bool ValidatedValueWasSet { get; private set; }

        private object _validatedValue;
        public object ValidatedValue
        {
            get
            {
                return _validatedValue;
            }
            set
            {
                if (!ValidatedValueWasSet)
                {
                    _validatedValue = value;
                    ValidatedValueWasSet = true;
                }
            }
        }
    }
}
