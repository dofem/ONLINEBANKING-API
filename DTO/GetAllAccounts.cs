using KINGDOMBANKAPI.API.Enums.AccountEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KINGDOMBANKAPI.BLL.DTO
{
    public class GetAllAccounts
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountFullName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public AccountClass AccountClass { get; set; }
        public string AccountNumberGenerated { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }

    }
}
