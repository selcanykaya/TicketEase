using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketEase.Business.Operations.Event;
using TicketEase.Business.Operations.Event.Dtos;
using TicketEase.Data.Enums;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;
using TicketEase.WebApi.Jwt;
using System.Security.Claims;
using TicketEase.WebApi.Filters;
using TicketEase.Business.Exceptions;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // Sadece Admin ve Organizer ekleyebilir
        [HttpPost("add")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> AddEvent(EventRequestModel request)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid event model.");

            var organizerId = int.Parse(User.Claims.First(c => c.Type == JwtClaimNames.Id).Value);

            var dto = new AddEventDto
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EventType = request.EventType,
                VenueId = request.VenueId,
                OrganizerId = organizerId
            };

            var result = await _eventService.AddEvent(dto);
            return result.Success
                ? Ok(new { result.Message })
                : BadRequest(new { result.Message });
        }

        // GET metodları herkese açık
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEvent(int id)
        {
            var result = await _eventService.GetByIdAsync(id);
            if (!result.Success) throw new NotFoundException(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _eventService.GetAllAsync();
            if (!result.Success) throw new ValidationException(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedEvents(int page = 1, int pageSize = 10, EventType? eventType = null)
        {
            var result = await _eventService.GetPagedEvents(page, pageSize, eventType);
            if (!result.Success) throw new NotFoundException(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchEvents(string nameStartsWith)
        {
            var result = await _eventService.SearchEventsByName(nameStartsWith);
            if (!result.Success) throw new ValidationException(result.Message);
            if (!result.Data.Any()) throw new NotFoundException("No events found.");

            return Ok(result.Data);
        }

        // Sadece Admin ve Organizer
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _eventService.GetByIdAsync(id);
            if (!existingEvent.Success) throw new NotFoundException(existingEvent.Message);

            var userId = int.Parse(User.Claims.First(c => c.Type == JwtClaimNames.Id).Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (userRole == "Organizer" && existingEvent.Data.OrganizerId != userId)
                throw new ForbiddenException("You can only delete your own events.");

            var result = await _eventService.DeleteAsync(id);
            if (!result.Success) throw new ValidationException(result.Message);

            return NoContent();
        }

        // Sadece Admin ve Organizer
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin,Organizer")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid event update model.");

            var existingEvent = await _eventService.GetByIdAsync(id);
            if (!existingEvent.Success) throw new NotFoundException("Event not found.");

            var userId = int.Parse(User.Claims.First(c => c.Type == JwtClaimNames.Id).Value);
            var userRole = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (userRole == "Organizer" && existingEvent.Data.OrganizerId != userId)
                throw new ForbiddenException("You can only modify your own events.");

            var dto = new UpdateEventDto
            {
                Name = model.Name,
                Description = model.Description,
                StartDate = model.StartDate,
                EventType = model.EventType,
                VenueId = model.VenueId
            };

            var result = await _eventService.UpdateEvent(id, dto);
            if (!result.Success) throw new ValidationException(result.Message);

            return Ok(new { result.Message });
        }


        // GET: api/Event/{eventId}/participants
        [HttpGet("{eventId}/participants")]
        public async Task<IActionResult> GetParticipants(int eventId)
        {
            var participants = await _eventService.GetEventParticipantsAsync(eventId);
            return Ok(participants);
        }
    }
}