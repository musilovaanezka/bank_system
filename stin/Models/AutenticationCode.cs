using System.ComponentModel.DataAnnotations;

namespace stin.Models
{
    public class AutenticationCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string Code { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
