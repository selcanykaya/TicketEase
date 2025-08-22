using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Order;
using TicketEase.Business.Operations.Order.Dtos;
using TicketEase.WebApi.Filters;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrder(OrderRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var dto = new AddOrderDto { UserId = model.UserId, TicketIds = model.TicketIds };
            var result = await _orderService.AddOrder(dto);

            if (!result.Success) return BadRequest(new { result.Message });

            return Ok(new { result.Message, OrderId = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.Success) return NotFound(new { result.Message });
            return Ok(result.Data);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            if (!result.Success) return BadRequest(new { result.Message });
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result.Success) return NotFound(new { result.Message });
            return NoContent();
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingOrder = await _orderService.GetByIdAsync(id);
            if (!existingOrder.Success) return NotFound(new { existingOrder.Message });

            var dto = new UpdateOrderDto
            {
                OrderDate = model.OrderDate,
                Status = model.Status,
                UserId = model.UserId,
                TicketIds = model.TicketIds ?? new List<int>()
            };

            var result = await _orderService.UpdateOrder(id, dto);
            if (!result.Success) return BadRequest(new { result.Message });

            return Ok(new { result.Message });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            if (!result.Success) return NotFound(new { result.Message });
            return Ok(result.Data);
        }
    }
}
