using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Order;
using TicketEase.Business.Operations.Order.Dtos;
using TicketEase.Business.Types;
using TicketEase.WebApi.Filters;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;
using TicketEase.Business.Exceptions;

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
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid order model.");

            var dto = new AddOrderDto { UserId = model.UserId, TicketIds = model.TicketIds };
            var result = await _orderService.AddOrder(dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid order update model.");

            var dto = new UpdateOrderDto
            {
                OrderDate = model.OrderDate,
                Status = model.Status,
                UserId = model.UserId,
                TicketIds = model.TicketIds ?? new List<int>()
            };

            var result = await _orderService.UpdateOrder(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
