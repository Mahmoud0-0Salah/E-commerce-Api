using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public record CateogryForManipulationDto
    {
        [Required(ErrorMessage = "Cateogry name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        public IEnumerable<ProductForCreationDto>? Product { get; set; }
    }
}
