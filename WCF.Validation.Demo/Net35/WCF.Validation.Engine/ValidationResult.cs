using System.Collections.Generic;

namespace WCF.Validation.Engine
{
    /// <summary>
    /// Copy from .NET 4.0 source code
    /// </summary>
    public class ValidationResult
    {
        // Fields
        public static readonly ValidationResult Success;

        // Methods
        protected ValidationResult(ValidationResult validationResult)
        {
            ErrorMessage = validationResult.ErrorMessage;
            MemberNames = validationResult.MemberNames;
        }

        public ValidationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            ErrorMessage = errorMessage;
            MemberNames = memberNames;
        }

        // Properties
        public string ErrorMessage { get; set; }

        public IEnumerable<string> MemberNames { get; private set; }
    }
}
