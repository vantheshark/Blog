using NValidator;

namespace WCF.Validation.Contract.Validators.Rules
{
    public class OrderValidator : CompositeValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
