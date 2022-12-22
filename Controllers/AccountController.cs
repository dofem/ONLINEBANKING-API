using AutoMapper;
using KINGDOMBANKAPI.API.DTO;
using KINGDOMBANKAPI.API.Model;
using KINGDOMBANKAPI.API.Service.Implementation;
using KINGDOMBANKAPI.API.Service.Interface;
using KINGDOMBANKAPI.BLL.DTO;
using KINGDOMBANKAPI.BLL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KINGDOMBANKAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly Response _response;
        private readonly IMailService _mailService;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
            _response = new Response();
        }

        [HttpPost]
        [Route("Register_new_Account")]
        public ActionResult<Response> RegisterNewAccount([FromBody] CreateAccount createAccount)
        {
            try
            {
                if (createAccount == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var account = _mapper.Map<Account>(createAccount);
                _accountService.CreateNewAccount(account, createAccount.Pin, createAccount.ConfirmPin);
                _response.ResponseCode = "00";
                _response.ResponseMessage = "Account Created Successfully";
                _response.Data = account;
            } 
            catch (Exception ex) 
            {
                _response.ResponseCode = "99";
                _response.ResponseMessage = ex.Message;
                _response.ResponseMessage = "Something went wrong, Account could not be created";
            }
            return _response;
        }
       

        [HttpPut]
        [Route("Reset-Account-Pin")]
        public ActionResult<Response> UpdateAccount([FromBody] UpdatePin updatePin,string Pin,string AccountNumber)
        {
            try
            {
                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }
                var account = _mapper.Map<Account>(updatePin);
                var UpdatedAccount = _accountService.UpdatePin(account, updatePin.Pin, updatePin.AccountNumberGenerated);
                _response.ResponseCode = "00";
                _response.ResponseMessage = "Pin reset Successfully";
                _response.Data = UpdatedAccount;              
            }
            catch
            {
                _response.ResponseCode = "99";
                _response.ResponseMessage = "Something went Wrong,Pin reset could not be processed";

            }
            return _response;
            
        }
    }


}

