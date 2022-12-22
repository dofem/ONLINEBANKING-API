using KINGDOMBANKAPI.API.Model;

namespace KINGDOMBANKAPI.API.Service.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
