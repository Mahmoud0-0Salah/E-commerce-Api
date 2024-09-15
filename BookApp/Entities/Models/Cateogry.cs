using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Cateogry
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public  ICollection<Product>? Product { get; set; }
    }
}
