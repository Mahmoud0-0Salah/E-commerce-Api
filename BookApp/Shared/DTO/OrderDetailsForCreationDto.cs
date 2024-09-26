using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public record OrderDetailsForCreationDto
    {
        [Required]
        public int ?ProductId { get; set; }
        [Required]
        public int? CateogryId { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
