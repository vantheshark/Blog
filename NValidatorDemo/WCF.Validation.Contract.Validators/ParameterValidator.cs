using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WCF.Validation.Contract.Validators
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class ParameterValidatorAttribute : Attribute, IOperationBehavior, IContractBehavior
    {
        #region -- IOperationBehavior Members --
        public void Validate(OperationDescription description)
        {
        }

        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
        {
            dispatch.ParameterInspectors.Add(new ParameterValidatorBehavior());
        }
        #endregion

        #region -- IContractBehavior Members --
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            foreach (DispatchOperation op in dispatchRuntime.Operations)
            {
                op.ParameterInspectors.Add(new ParameterValidatorBehavior());
            }
        }
        #endregion
    }
}
