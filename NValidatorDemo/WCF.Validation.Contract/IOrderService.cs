using System.ServiceModel;

namespace WCF.Validation.Contract
{
    [ServiceContract]
    public interface IOrderService
    {
        [FaultContract(typeof(ValidationFault))]
        [OperationContract]
        void CreateOrder(Order order);
    }
}
