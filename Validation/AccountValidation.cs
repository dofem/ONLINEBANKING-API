using FluentValidation;
using KINGDOMBANKAPI.BLL.Model;

namespace KINGDOMBANKAPI.API.Validation
{
    public class AccountValidation : AbstractValidator<Account>
    {
        public AccountValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.PhoneNumber).Length(11);
            RuleFor(x => x.AccountClass).NotNull();
            RuleFor(x => x.AccountType).NotNull();
            RuleFor(x => x.AccountNumberGenerated).NotNull().Length(10);
            RuleFor(x => x.CurrentAccountBalance).NotEmpty();
        }
    }
}
