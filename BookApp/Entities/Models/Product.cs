using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public  int Price { get; set; }

        [Required]
        public string? Description { get; set; }

        [ForeignKey("Cateogry")]
        public int CateogryId { get; set; }
        public  Cateogry Cateogry { get; set; }

    }
}
