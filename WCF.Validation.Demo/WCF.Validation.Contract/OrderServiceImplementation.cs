using System.ServiceModel;
using System.Text;
using WCF.Validation.Engine;

namespace WCF.Validation.Contract
{
    public class OrderServiceImplementation : IOrderService
    {
        [ParameterValidator]
        public void CreateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                var error = new StringBuilder();
                foreach (var e in ModelState.Errors)
                {
                    error.Append(string.Format("Validation error on {0}:{1}\n", e.MemberName, e.Message));
                }
                throw new FaultException(error.ToString());
            }
        }

        public ModelState ModelState { get; set; }
    }
}
