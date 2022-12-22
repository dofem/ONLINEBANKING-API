using System.ComponentModel.DataAnnotations;

namespace KINGDOMBANKAPI.API.Model
{
    public class AuthenticateAccount
    {
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be must than 4 digit")]
        public string Pin { get; set; }
    }
}
