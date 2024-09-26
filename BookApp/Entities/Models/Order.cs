using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{

    public enum OrderState
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
    public class Order
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public string UserId { get; set; }
        public string Adress { get; set; }
        public OrderState OrderState { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }
    }
}
