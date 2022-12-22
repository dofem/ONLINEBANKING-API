using KINGDOMBANKAPI.API.Enums.TransactionEnum;
using KINGDOMBANKAPI.API.Model;
using KINGDOMBANKAPI.API.Service.Interface;
using KINGDOMBANKAPI.BLL.Model;
using KINGDOMBANKAPI.DAL.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Principal;

namespace KINGDOMBANKAPI.API.Service.Implementation
{
    public class TransactionService  : ITransactionService
    {
        private ApplicationDbContext _context;
        ILogger<TransactionService> _logger;
        private AppSettings _appsettings;
        private static string _kingdomBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(ApplicationDbContext context,ILogger<TransactionService> logger,IOptions<AppSettings> options,IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _appsettings = options.Value;
            _kingdomBankSettlementAccount = _appsettings.KingdomBankSettlementAccount;
            _accountService = accountService;

        }

        public Response FindTRansactionbyDate(DateTime date)
        {   
            Response response = new Response();
            var transaction = _context.Transactions.Where(x=>x.TransactionDate== date).ToList();
            if (transaction.Any())
            { 
                response.Data = transaction;
                response.ResponseCode = "00";
                response.ResponseMessage = string.Empty;
            }
            else
            {
                response.Data = null;
                response.ResponseCode = "99";
                response.ResponseMessage = "No transaction on this day";
            }
            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response= new Response();
            Transaction transaction = new Transaction();

            //Validating the accountNumber
            if(_accountService.AccountNumberIsAuthenticated(AccountNumber, TransactionPin))
                try
                {
                    

                    var SourceAccount = _accountService.GetAccountbyId(7);
                    var DestinationAccount = _accountService.GetAccountbyAccountNumber(AccountNumber);

                    //updating the account balance
                    SourceAccount.CurrentAccountBalance -= Amount;
                    DestinationAccount.CurrentAccountBalance += Amount;

                    //check if the update has been made in the database
                    if ((_context.Entry(SourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(DestinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        //transaction has been made successfully
                        transaction.TransactionStatus = Enums.TransactionEnum.TransStatus.Successful;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction made successfully";
                        response.Data = null;
                    }
                    else
                    {
                        //Failed Transaction
                        transaction.TransactionStatus = TransStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Failed";
                        response.Data = null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An Error Occured....=>{ex.Message}");
                }
                transaction.TransactionType = TransType.Deposit;
                transaction.TransactionSourceAccount = "2000000000";
                transaction.TransactionDestinationAccount = AccountNumber;
                transaction.TransactionAmount = Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionDescription =
                    $"SUCCESSFUL TRANSACTION ON ACCOUNT NUMBER => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} CREATED ON " +
                    $"{JsonConvert.SerializeObject(transaction.TransactionDate)} FOR AMOUNT=>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                    $"TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)},TRANSACTION STATUS=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";
            

             _context.Transactions.Add( transaction );
             _context.SaveChanges();

             return response;
        }

        public Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            //make Fund Transfer
            Response response = new Response();
            Transaction transaction = new Transaction();

            //validating source account by authenticating
            var authUser = _accountService.AccountNumberIsAuthenticated(FromAccount, TransactionPin);
            if (authUser == null)
                throw new Exception("Invalid Credentials");

            try
            {
                //Our source account will be fetched from the database
                var sourceAccount = _accountService.GetAccountbyAccountNumber(FromAccount);
                if (sourceAccount == null)
                {
                    throw new Exception("Account Number does not exist");
                }
                var destinationAccount = _accountService.GetAccountbyAccountNumber(ToAccount);

                //Now let us update our account balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is update
                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //Transaction is Successful
                    transaction.TransactionStatus = TransStatus.Successful;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Fund Transfer Successful";
                    response.Data = null;
                }
                else
                {
                    //Failed Transaction
                    transaction.TransactionStatus = TransStatus.Failed;
                    response.ResponseCode = "99";
                    response.ResponseMessage = "Fund Transfer Failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured....=>{ex.Message}");
            }

            //set other props of transaction
            transaction.TransactionType = TransType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionDescription = $"NEW TRANSACTION FROM =>{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $"TO DESTINATION ACCOUNT=> {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} CREATED ON DATE" +
                $"{JsonConvert.SerializeObject(transaction.TransactionDate)} FOR AMOUNT=>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)},TRANSACTION STATUS=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;

        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Transaction transaction = new Transaction();

            //Validating the accountNumber
            var UserAuth = _accountService.AccountNumberIsAuthenticated(AccountNumber, TransactionPin);
            if (UserAuth == null)
            {
                response.ResponseMessage = "Invalid Credential";
                throw new ApplicationException($"Invalid Credentials");
            }
            else
            {
                try
                {


                    var DestinationAccount = _accountService.GetAccountbyId(7);
                    var SourceAccount = _accountService.GetAccountbyAccountNumber(AccountNumber);

                    //updating the account balance
                    SourceAccount.CurrentAccountBalance -= Amount;
                    DestinationAccount.CurrentAccountBalance += Amount;

                    //check if the update has been made in the database
                    if ((_context.Entry(SourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(DestinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        //transaction has been made successfully
                        transaction.TransactionStatus = Enums.TransactionEnum.TransStatus.Successful;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction made successfully";
                        response.Data = null;
                    }
                    else
                    {
                        //Failed Transaction
                        transaction.TransactionStatus = TransStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Failed";
                        response.Data = null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An Error Occured....=>{ex.Message}");
                }
                transaction.TransactionType = TransType.Withdrawal;
                transaction.TransactionSourceAccount = AccountNumber;
                transaction.TransactionAmount = Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionDescription =
                    $"SUCCESSFUL TRANSACTION ON ACCOUNT NUMBER => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} CREATED ON " +
                    $"{JsonConvert.SerializeObject(transaction.TransactionDate)} FOR AMOUNT=>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                    $"TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)},TRANSACTION STATUS=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";
            }

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }
    }
}
