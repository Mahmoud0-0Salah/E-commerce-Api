using Entities.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{

    public enum ProductState
    {
        Pending,
        Accepted,
        Cancelled
    }
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public  int Price { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public string? Description { get; set; }
       
        [Required]
        public ProductState ProductState { get; set; }

        [ForeignKey("Cateogry")]
        public int CateogryId { get; set; }
        public  Cateogry Cateogry { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }

    }
}
