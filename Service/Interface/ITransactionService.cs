using KINGDOMBANKAPI.API.Model;

namespace KINGDOMBANKAPI.API.Service.Interface
{
    public interface ITransactionService
    {
        Response FindTRansactionbyDate( DateTime date );
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeFundTransfer (string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
