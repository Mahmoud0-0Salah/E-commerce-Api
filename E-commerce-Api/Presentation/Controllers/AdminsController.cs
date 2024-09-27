using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminsController : ControllerBase
    {
        IServiceManager _serviceManager;
        public AdminsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingProducts([FromQuery] ProductParameters productParameters)
        {
            var PageResult = await _serviceManager.ProductService.GetPendingProductsAsync(false, productParameters);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(PageResult.MetaData));

            return Ok(PageResult);
        }

        [HttpPut("{cateogryId}/{productId}")]
        public async Task<IActionResult> ChangeProductState(int cateogryId, int productId, [FromQuery] string productState)
        {
            if (!Enum.TryParse<ProductState>(productState, true, out var parsedState))
            {
                return BadRequest("Invalid order state.");
            }
            await _serviceManager.ProductService.ChangeProductState(cateogryId, productId, parsedState);
            return Ok();
        }
    }
}
