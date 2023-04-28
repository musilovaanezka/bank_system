using System.ComponentModel.DataAnnotations;

namespace stin.Models
{
    public class Klient
    {
        [Key]
        [StringLength(25)]
        public string UcetNum { get; set; }


        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
