using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Venue;
using TicketEase.Business.Operations.Venue.Dtos;
using TicketEase.Data.Enums;
using TicketEase.WebApi.Filters;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(VenueRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = new AddVenueDto
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Capacity = model.Capacity
            };

            await _venueService.AddVenue(dto);

            return Ok(new { Message = "Venue added successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenue(int id)
        {
            var venue = await _venueService.GetByIdAsync(id);
            return Ok(venue);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueService.GetAllAsync();
            return Ok(venues);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            await _venueService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVenue(int id, UpdateVenueRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = new UpdateVenueDto
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Capacity = model.Capacity
            };

            await _venueService.UpdateVenue(id, dto);

            return Ok(new { Message = "Venue updated successfully." });
        }

        [HttpGet("sorted-by-capacity")]
        public async Task<IActionResult> GetVenuesSortedByCapacity(bool descending = false)
        {
            var venues = await _venueService.GetVenuesSortedByCapacity(descending);
            return Ok(venues);
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetVenuesByCity(City city)
        {
            var venues = await _venueService.GetVenuesByCity(city);
            return Ok(venues);
        }
    }
}

