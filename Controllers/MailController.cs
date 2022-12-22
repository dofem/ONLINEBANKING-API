using KINGDOMBANKAPI.API.Model;
using KINGDOMBANKAPI.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KINGDOMBANKAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
       private readonly IMailService _mailService;
        

       public MailController(IMailService mailService)
       {
         _mailService = mailService;
       }
      
        [HttpPost("send")]
       public async Task<IActionResult> SendMail([FromForm] MailRequest request)
       {
         try
         {
          await _mailService.SendEmailAsync(request);
          return Ok();
          }
         catch (Exception ex)
         {
          throw;
         }

            
        }
    }
}
