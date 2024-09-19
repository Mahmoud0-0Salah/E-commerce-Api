using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.ActionFilters;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service) => _service = service;

        [HttpPost]
        [ValidationFilterAttribute]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPost("login")]
        [ValidationFilterAttribute]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForRegistration)
        {
            if (!await _service.AuthenticationService.ValidateUser(userForRegistration))
                return Unauthorized("Invalid user data");

            var res = await _service.AuthenticationService.CreateToken(true, userForRegistration.rememberMe);
            
            return Ok(res);
        }

        [HttpPost("refresh")]
        [ValidationFilterAttribute]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
