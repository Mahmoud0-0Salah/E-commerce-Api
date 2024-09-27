using Entities.LinkModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.ActionFilters;
using Shared.DTO;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        IServiceManager _serviceManager;
        public UsersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [ValidateMediaTypeAttribute]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters userParameters)
        {
            var linkParams = new LinkParameters<UserParameters>(userParameters, HttpContext);

            var PageResult = await _serviceManager.UserService.GetAllUsersAsync(false, linkParams);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(PageResult.MetaData));

            return Ok(PageResult.Users.HasLinks ? PageResult.Users.LinkedEntities : PageResult.Users.Entities);
        }


        [HttpGet("{id}",Name = "GetUserById")]
        public async Task<IActionResult> GetUser(string id)
        {
            return Ok( await _serviceManager.UserService.GetUser(id));
        }

        [HttpPut("{id}")]
        [ValidationFilterAttribute]
        public async Task<IActionResult> UpdateUser(string id,[FromBody] UserDtoForUpdate userDtoForUpdate)
        {
            var result = await _serviceManager.UserService.UpdateUser(userDtoForUpdate,id);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _serviceManager.UserService.DeleteUser(id);
            return NoContent();

        }

    }
}
