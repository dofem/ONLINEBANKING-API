using AutoMapper;
using KINGDOMBANKAPI.BLL.DTO;
using KINGDOMBANKAPI.BLL.Model;

namespace KINGDOMBANKAPI.API.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<CreateAccount, Account>().ReverseMap();
            CreateMap<GetAllAccounts, Account>().ReverseMap();
            CreateMap<UpdateAccounts, Account>().ReverseMap();
        }
    }
}
