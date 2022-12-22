using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KINGDOMBANKAPI.API.DTO
{
    public class UpdatePin
    {
        [ReadOnly(true)]
        public string AccountNumberGenerated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be must than 4 digit")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
    }
}
