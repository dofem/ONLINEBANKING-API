using KINGDOMBANKAPI.API.Model;
using KINGDOMBANKAPI.BLL.DTO;
using KINGDOMBANKAPI.BLL.Model;

namespace KINGDOMBANKAPI.API.Service.Interface
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAllAccounts();
        Account CreateNewAccount (Account account,string Pin,string ConfirmPin);
        Account UpdateAccount(Account account,string AccountNumber,string Pin);
        Account DeleteAccount(string AccountNumber);
        bool AccountNumberIsAuthenticated(string AccountNumber, string pin);
        Account GetAccountbyId(int Id);
        Account GetAccountbyAccountNumber(string AccountNumber);
        Account UpdatePin(Account account,string AccountNumber,string Pin);
    }
}
