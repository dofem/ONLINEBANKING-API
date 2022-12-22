using FluentValidation;
using KINGDOMBANKAPI.API.Model;

namespace KINGDOMBANKAPI.API.Validation
{
    public class TransactionValidation : AbstractValidator<Transaction>
    {
        public TransactionValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.TransactionSourceAccount).Length(10);
            RuleFor(x => x.TransactionDestinationAccount).Length(10);
            RuleFor(x => x.TransactionAmount).GreaterThan(0);
            
        }
    }
}
