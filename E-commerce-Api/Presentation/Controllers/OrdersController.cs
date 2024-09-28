using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [Route("api/Users/{UserId}/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IServiceManager _serviceManager;
        public OrdersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [ValidationFilter]
        public async Task<IActionResult> CreateOrder(string UserId, OrderForCreationDto order)
        {
            var CreatedOrder = await _serviceManager.OrderService.CreateOrder(UserId, order);

            return CreatedAtRoute("GetOrdertById", new { UserId = UserId, OrderId = CreatedOrder.Id }, CreatedOrder);

        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders(string UserId ,[FromQuery] OrderParameters orderParameters)
        {
            var result = await _serviceManager.OrderService.GetAllOrdersAsync(UserId, orderParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));
            
            return Ok(result);
        }

        [HttpGet("{OrderId}",Name = "GetOrdertById")]
        public async Task<IActionResult> GetOrder(string UserId,int OrderId)
        {
            return Ok(await _serviceManager.OrderService.GetOrderAsync(UserId, OrderId, false));
        }


        [HttpDelete("{OrderId}")]
        public async Task<IActionResult> DeleteOrder(string UserId, int OrderId)
        {
            await _serviceManager.OrderService.DeleteOrder(UserId, OrderId);

            return NoContent();
        }
    }
}
