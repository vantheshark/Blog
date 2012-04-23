using NValidator;
using WCF.Validation.Contract.Validators;
using WCF.Validation.Contract.Validators.Rules;

namespace WCF.Validation.Demo.Server
{
    internal class BootStrapper
    {
        public static void Start()
        {
            ValidatorFactory.Config.DefaultValidationBuilderType = typeof (CustomValidationBuilder<,>);

            ValidatorFactory.Current.Register<OrderValidator>(true);
            ValidatorFactory.Current.Register<OrderDetailValidator>(true);
        }
    }
}