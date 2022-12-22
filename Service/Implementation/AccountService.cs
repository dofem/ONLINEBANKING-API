using KINGDOMBANKAPI.API.Model;
using KINGDOMBANKAPI.API.Service.Interface;
using KINGDOMBANKAPI.BLL.Model;
using KINGDOMBANKAPI.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace KINGDOMBANKAPI.API.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Account CreateNewAccount(Account account,string Pin,string ConfirmPin)
        {
            var acct = _context.Accounts.Any(x => x.Email == account.Email);
            if (acct==true)
            {
                //Console.WriteLine("Already exist");
                throw new ApplicationException("An Account Number Already Exist with the Email");
            }
            Byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinSalt,out pinHash);
            account.PinSalt = pinSalt;
            account.PinHash = pinHash;

            account.GenerateAccount();
            
            
           

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
           
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            var account = _context.Accounts.ToList();
            return account;
        }

        public Account UpdateAccount(Account account,string AccountNumber,string Pin = null)
        {
            var AccountTobeUpdated = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (AccountTobeUpdated == null)
            {
                throw new ApplicationException($"This AccountNumber {AccountNumber} doesnt exist in our database");
            }
            if(!string.IsNullOrWhiteSpace(account.PhoneNumber))
            { 
                if(_context.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                    throw new ApplicationException($"This PhoneNumber{account.PhoneNumber} already exist");
                AccountTobeUpdated.PhoneNumber = account.PhoneNumber;                      
            }
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_context.Accounts.Any(x => x.Email == account.Email))
                    throw new ApplicationException($"This Email Address{account.Email} already exist");
                AccountTobeUpdated.Email = account.Email;
            }
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                AccountTobeUpdated.PinHash= pinHash;
                AccountTobeUpdated.PinSalt= pinSalt;
            }
            AccountTobeUpdated.DateUpdated = DateTime.Now;

            _context.Accounts.Update(account);
            _context.SaveChanges();
            return account;

        }



        private static void CreatePinHash(string Pin, out Byte[] PinHash ,out Byte[] PinSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                PinHash = hmac.Key;
                PinSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
            }
        }

        private bool VerifyPinHash(string Pin, Byte[] PinHash, Byte[] PinSalt)
        {
            using (var hmac = new HMACSHA512(PinSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                return computedHash.SequenceEqual(PinHash);
            }
        }

        public Account DeleteAccount(string AccountNumber)
        {
           var account =  _context.Accounts.Where(x=>x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account != null)
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            if (account == null)
                return null;
            return account;
        }

        public bool AccountNumberIsAuthenticated(string AccountNumber, string pin)
        {
            var account = _context.Accounts.Where(x=> x.AccountNumberGenerated== AccountNumber).FirstOrDefault();
            if (account == null) return false;
               

            if (!VerifyPinHash(pin,account.PinHash,account.PinSalt)) return false;

            return true;
               
        }

        public Account GetAccountbyId(int Id)
        {
            var account = _context.Accounts.Where(x => x.Id == Id).FirstOrDefault();
            if (account == null)
                return null;
            return account;
        }

        public Account GetAccountbyAccountNumber(string AccountNumber)
        {
            var account = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;
            return account;
        }

        public Account UpdatePin(Account account, string Pin, string AccountNumber)
        {
            var AccountTobeUpdated = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (AccountTobeUpdated == null)
                return null;
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                AccountTobeUpdated.PinHash = pinHash;
                AccountTobeUpdated.PinSalt = pinSalt;
            }
          

            _context.Accounts.Update(account);
            _context.SaveChanges();
            return account;
        }
    }

}
