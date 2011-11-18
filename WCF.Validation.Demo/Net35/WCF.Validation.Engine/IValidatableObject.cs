using System.Collections.Generic;

namespace WCF.Validation.Engine
{
    /// <summary>
    /// Copy from .NET 4.0 source code
    /// </summary>
    public interface IValidatableObject
    {
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
