using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class UserDtoForUpdate
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Adress { get; set; }
        [Required]
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
