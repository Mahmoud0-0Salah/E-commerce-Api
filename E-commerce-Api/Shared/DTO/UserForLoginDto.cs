using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public record UserForLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; init; }
        [Required(ErrorMessage = "Password name is required")]
        public string? Password { get; init; }
        public bool rememberMe { get; init; } = false;
    }
}
