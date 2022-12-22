using KINGDOMBANKAPI.API.Enums.AccountEnum;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KINGDOMBANKAPI.BLL.Model
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //public string AccountFullName => FirstName + " " + LastName;
        public string AccountFullName
        {
            get { return FirstName + " " + LastName; }
        }
        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public AccountClass AccountClass { get; set; }
        public string AccountNumberGenerated { get; set; } 
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]

        public byte[] PinSalt { get; set; }


        public string GenerateAccount()
        {
            Random random = new Random();
            int i;
            switch (AccountType)
            {
                case AccountType.Staff:
                    if (AccountClass == AccountClass.Savings)
                    {
                        var AccountPrefix = "2020";
                        for (i = 1; i < 7; i++)
                        {
                            AccountNumberGenerated = AccountPrefix += random.Next(0, 9).ToString();
                        }
                        Console.WriteLine(AccountNumberGenerated);
                    }

                    if (AccountClass == AccountClass.Current)
                    {
                        var AccountPrefix = "1020";
                        for (i = 1; i < 7; i++)
                        {
                            AccountNumberGenerated = AccountPrefix += random.Next(0, 9).ToString();
                        }
                        Console.WriteLine(AccountNumberGenerated);
                    }
                    break;

                case AccountType.Customer:
                    if (AccountClass == AccountClass.Savings)
                    {
                        var AccountPrefix = "2";
                        for (i = 1; i < 10; i++)
                        {
                            AccountNumberGenerated = AccountPrefix += random.Next(0, 9).ToString();
                        }
                        Console.WriteLine(AccountNumberGenerated);
                    }

                    if (AccountClass == AccountClass.Current)
                    {
                        var AccountPrefix = "10";
                        for (i = 1; i < 9; i++)
                        {
                            AccountNumberGenerated = AccountPrefix += random.Next(0, 9).ToString();
                        }
                        Console.WriteLine(AccountNumberGenerated);
                    }

                    if (AccountClass == AccountClass.Corporate)
                    {
                        var AccountPrefix = "10101";
                        for (i = 1; i < 6; i++)
                        {
                            AccountNumberGenerated = AccountPrefix += random.Next(0, 9).ToString();
                        }
                        Console.WriteLine(AccountNumberGenerated);
                    }
                    if (AccountClass == AccountClass.Government)
                    {
                        var AccountPrefix = "10555";
                        for (i = 1; i < 6; i++)
                        {
                            AccountNumberGenerated = AccountPrefix += random.Next(0, 9).ToString();
                        }
                        Console.WriteLine(AccountNumberGenerated);
                    }
                    break;

                    default:
                    {
                        AccountNumberGenerated = "2222211111";
                    }
                    break;

            
            }
             return AccountNumberGenerated;
        }

    } 
};