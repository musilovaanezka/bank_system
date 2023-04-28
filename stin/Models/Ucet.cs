using System.ComponentModel.DataAnnotations;

namespace stin.Models
{
    public class Ucet
    {
        [Key]
        public int Id { get; set; }

        [StringLength(25)]
        public string UcetNum { get; set; }

        [StringLength(5)]
        public string Mena { get; set; }

        public int hodnota { get; set; }
    }
}
