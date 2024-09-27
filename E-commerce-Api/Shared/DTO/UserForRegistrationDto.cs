using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public record UserForRegistrationDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(15)]
        public string? FirstName { get; init; }

        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(15)]
        public string? LastName { get; init; }

        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }

        public string? PhoneNumber { get; init; }
        public ICollection<string>? Roles { get; init; }
        public string? PhotoUrl { get; set; }
        public string? Adress { get; set; }

    }
}
