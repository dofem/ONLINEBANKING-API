using KINGDOMBANKAPI.API.Enums.TransactionEnum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KINGDOMBANKAPI.API.Model
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference => $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";
        [Precision(18, 2)]
        public decimal TransactionAmount { get; set; }
        public TransStatus TransactionStatus { get; set; }  //this is an enum
        public bool IsSuccessful => TransactionStatus.Equals(TransStatus.Successful);  //this depends on the value of transaction status
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionDescription { get; set; }
        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

       
    }

   

  
}
    

