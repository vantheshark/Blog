using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WCF.Validation.Engine
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ParameterValidatorAttribute : Attribute, IOperationBehavior
    {
        public ParameterValidatorAttribute()
        {
            ThrowErrorOnFirstError = false;
            ThrowErrorAfterValidation = true;
        }

        public bool ThrowErrorOnFirstError { get; set; }
        public bool ThrowErrorAfterValidation { get; set; }

        void IOperationBehavior.Validate(OperationDescription description)
        {
        }

        void IOperationBehavior.AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
        }

        void IOperationBehavior.ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
        {
        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
        {
            dispatch.ParameterInspectors.Add(new ParameterValidatorBehavior(ThrowErrorOnFirstError, ThrowErrorAfterValidation));
        }
    }
}