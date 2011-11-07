using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WCF.Validation.Engine
{
    [DataContract]
    public class ValidationFault
    {
        public ValidationFault(IEnumerable<ModelValidationResult> result)
        {
            var error = new StringBuilder();
            foreach (var e in result)
            {
                error.AppendLine();
                error.Append(string.Format("Validation error on {0}: {1}", e.MemberName, e.Message));
                error.AppendLine();
                error.AppendLine();
            }
            ErrorMessage = error.ToString();
        }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
