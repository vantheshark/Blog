using System.ServiceModel;
using WCF.Validation.Engine;

namespace WCF.Validation.Contract
{
    [ServiceContract]
    public interface IOrderService : IHasModelStateService
    {
        [FaultContract(typeof(ValidationFault))]
        [OperationContract]
        void CreateOrder(Order order);
    }
}
