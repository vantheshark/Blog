using NValidator;

namespace WCF.Validation.Contract.Validators.Rules
{
    public class OrderDetailValidator : TypeValidator<OrderDetail>
    {
        public OrderDetailValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.ProductName)
                .StopOnFirstError()
                .NotNull()
                .Length(1, 5);
        }
    }
}
