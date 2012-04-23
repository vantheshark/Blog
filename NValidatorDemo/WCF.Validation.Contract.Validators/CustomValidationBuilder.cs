using System;
using System.Linq.Expressions;
using NValidator.Builders;

namespace WCF.Validation.Contract.Validators
{
    /// <summary>
    /// This class customizes the behavior of ValidationBuilder and return CustomValidationResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public class CustomValidationBuilder<T, TProperty> : ValidationBuilder<T, TProperty>
    {
        private object _validatedObject;
        public CustomValidationBuilder(Expression<Func<T, TProperty>> expression)
            : base(expression)
        {
        }

        protected override NValidator.ValidationResult FormatValidationResult(NValidator.ValidationResult result, string propertyChain)
        {
            var baseResult = base.FormatValidationResult(result, propertyChain);
            var customResult = new CustomValidationResult(baseResult);
            if (!customResult.ValidatedValueWasSet)
            {
                customResult.ValidatedValue = _validatedObject;
            }
            return customResult;
        }

        protected override object GetObjectToValidate(T value)
        {
            _validatedObject = base.GetObjectToValidate(value);
            return _validatedObject;
        }
    }
}
