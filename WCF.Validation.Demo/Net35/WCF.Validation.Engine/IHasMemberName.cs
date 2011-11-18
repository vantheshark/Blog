
namespace WCF.Validation.Engine
{
    /// <summary>
    /// This interface to support getting the MemberName from the Attribute
    /// If convert to .NET 4.0, this one should be remove since we can use ValidationAttribute.GetValidationResult builtin 4.0
    /// </summary>
    public interface IHasMemberName
    {
        string MemberName { get; }
    }
}
