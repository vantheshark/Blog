using System;
using System.ComponentModel.DataAnnotations;

namespace WCF.Validation.Engine
{
    public static class ValidationAttributeExtensions
    {
        /// <summary>
        /// Copy from .NET 4.0 source code
        /// </summary>
        public static ValidationResult GetValidationResult(this ValidationAttribute att, object value, ValidationContext context)
        {
            var memberName = context.MemberName;
            try
            {
                att.Validate(value, memberName);
            }
            catch (Exception ex)
            {
                //NOTE: The MemberName could be set after the IsValid method is executed
                if (att is IHasMemberName)
                {
                    memberName = (att as IHasMemberName).MemberName ?? context.MemberName;
                }
                return new ValidationResult(ex.Message, string.IsNullOrEmpty(memberName) ? null : new[] { memberName });
            }
            return null;
        }
    }
}
