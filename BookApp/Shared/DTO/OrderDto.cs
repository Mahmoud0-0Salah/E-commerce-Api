using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public record OrderDto
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public string UserId { get; set; }
        public string Adress { get; set; }
        public string OrderState { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
