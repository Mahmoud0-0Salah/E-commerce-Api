using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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
    [Authorize(Roles = "Delivery")]
    public class DeliveryController :  ControllerBase
    {
        IServiceManager _serviceManager;
        public DeliveryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPut("{UserId}/Orders/{OrderId}")]
        public async Task<IActionResult> ChangeOrderState(string UserId, int OrderId, [FromQuery] string orderState)
        {
            if (!Enum.TryParse<OrderState>(orderState, true, out var parsedState))
            {
                return BadRequest("Invalid order state.");
            }
            await _serviceManager.OrderService.ChangeOrderState(UserId, OrderId, parsedState);
            return NoContent();
        }


        [HttpGet("Pending-Orders")]
        public async Task<IActionResult> GetAllPendingOrdersAsync([FromQuery] OrderParameters orderParameters)
        {
            var result = await _serviceManager.OrderService.GetAllPendingOrdersAsync(orderParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));

            return Ok(result);
        }

        [HttpGet("Delivered-Orders")]
        public async Task<IActionResult> GetAllDeliveredOrdersAsync([FromQuery] OrderParameters orderParameters)
        {
            var result = await _serviceManager.OrderService.GetAllDeliveredOrdersAsync(orderParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));

            return Ok(result);
        }

        [HttpGet("Shipped-Orders")]
        public async Task<IActionResult> GetAllShippedOrdersAsync([FromQuery] OrderParameters orderParameters)
        {
            var result = await _serviceManager.OrderService.GetAllShippedOrdersAsync(orderParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));

            return Ok(result);
        }
    }
}
