using Microsoft.AspNetCore.Mvc;
using TicketEase.Business.Operations.Ticket;
using TicketEase.Business.Operations.Ticket.Dtos;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;
using TicketEase.Business.Exceptions; 
using Microsoft.AspNetCore.Authorization;
using TicketEase.WebApi.Filters;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTicket(TicketRequestModel request)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid ticket model.");

            var dto = new AddTicketDto
            {
                EventId = request.EventId,
                SeatNumber = request.SeatNumber,
                Price = request.Price,
                IsSold = request.IsSold
            };

            await _ticketService.AddTicket(dto);
            return Ok(new { Message = "Ticket successfully added." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id); 
            return Ok(ticket);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteAsync(id); 
            return NoContent();
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTicket(int id, UpdateTicketRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid ticket model.");

            var dto = new UpdateTicketDto
            {
                SeatNumber = model.SeatNumber,
                Price = model.Price,
                IsSold = model.IsSold,
                EventId = model.EventId
            };

            await _ticketService.UpdateTicket(id, dto); 
            return Ok(new { Message = "Ticket updated successfully." });
        }

        [HttpGet("is-sold/{ticketId}")]
        public async Task<IActionResult> IsTicketSold(int ticketId)
        {
            var isSold = await _ticketService.IsTicketSoldAsync(ticketId);
            return Ok(new { TicketId = ticketId, IsSold = isSold });
        }

        [HttpGet("sold-count/{eventId}")]
        public async Task<IActionResult> GetSoldTicketCount(int eventId)
        {
            var count = await _ticketService.CountSoldTicketsByEventAsync(eventId);
            return Ok(new { SoldTickets = count });
        }
    }
}
