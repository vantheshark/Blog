using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using NValidator;

namespace WCF.Validation.Contract.Validators
{
    public class ParameterValidatorBehavior : IParameterInspector
    {
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            // validate parameters before call
            var validationResults = new List<ValidationResult>();
            var serviceIntance = OperationContext.Current.InstanceContext.GetServiceInstance();
            if (serviceIntance != null)
            {
                foreach (var input in inputs)
                {
                    if (input != null)
                    {
                        var validator = ValidatorFactory.Current.GetValidatorFor(input.GetType());
                        if (validator == null)
                        {
                            ColorConsole.WriteLine(ConsoleColor.Yellow, "Validator for {0} not found.", input.GetType().Name);
                            continue;
                        }
                        validationResults.AddRange(validator.GetValidationResult(input));
                    }
                }
                if (validationResults.Count > 0)
                {
                    throw new FaultException<ValidationFault>(new ValidationFault(validationResults), "Validation error");
                }
            }
            return null;
        }
    } 
}
