using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public record OrderForCreationDto
    {
        [Required]
        public string Adress { get; set; }
        [Required]
        public ICollection<OrderDetailsForCreationDto>? OrderDetails { get; set; }

    }
}
