using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WCF.Validation.Contract
{
    [DataContract]
    public class ValidationFault
    {
        public ValidationFault(IEnumerable<dynamic> result)
        {
            var error = new StringBuilder();
            foreach (var e in result)
            {
                error.AppendLine();
                error.AppendLine(string.Format("Validation error on {0}: {1}", e.MemberName, e.Message));
                error.AppendLine(string.Format("You sent value: {0}", e.ValidatedValue));
                error.AppendLine();
                error.AppendLine();
            }
            ErrorMessage = error.ToString();
        }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
