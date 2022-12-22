using KINGDOMBANKAPI.API.Model;
using KINGDOMBANKAPI.API.Service.Implementation;
using KINGDOMBANKAPI.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace KINGDOMBANKAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        public Response _response;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _response= new Response();
        }


        [HttpPost]
        [Route("Top-Up-Deposit")]
        public ActionResult<Response> TopUpDeposit(string AccountNumber,decimal Amount,string Pin)
        {
            try
            {
                var transaction = _transactionService.MakeDeposit(AccountNumber, Amount, Pin);
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                _response.ResponseCode = "00";
                _response.ResponseMessage = "Transaction Processed Successfully";
                
            }
            catch (Exception ex)
            {
                _response.ResponseCode = "99";
                _response.ResponseMessage = ex.Message;
                _response.ResponseMessage = "Failed to process transaction";
            }
            return _response;
        }


        [HttpPost]
        [Route("Fund_Transfer")]
        public ActionResult<Response> MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            try
            {
                var transaction = (_transactionService.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                _response.ResponseCode = "00";
                _response.ResponseMessage = "Transaction Processed Successfully";
            }
            catch (Exception ex) 
            {
                _response.ResponseCode = "99";
                _response.ResponseMessage = ex.Message;
                _response.ResponseMessage = "Failed to process transaction";
            }
            return _response;
        }


    }
}
