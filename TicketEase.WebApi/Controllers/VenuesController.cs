using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Venue;
using TicketEase.Business.Operations.Venue.Dtos;
using TicketEase.Data.Enums;
using TicketEase.WebApi.Filters;
using TicketEase.Business.Exceptions;
using TicketEase.Business.Types;
using TicketEase.WebApi.Models.Update;
using TicketEase.WebApi.Models;

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
                throw new ValidationException("Invalid venue model.");

            var dto = new AddVenueDto
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Capacity = model.Capacity
            };

            await _venueService.AddVenue(dto);

            return Ok(new ServiceMessage { Success = true, Message = "Venue added successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenue(int id)
        {
            var venue = await _venueService.GetByIdAsync(id);
            if (venue == null)
                throw new NotFoundException("Venue not found.");

            return Ok(new ServiceMessage<VenueDto> { Success = true, Data = venue });
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueService.GetAllAsync();
            return Ok(new ServiceMessage<IEnumerable<VenueDto>> { Success = true, Data = venues });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            await _venueService.DeleteAsync(id);
            return Ok(new ServiceMessage { Success = true, Message = "Venue deleted successfully." });
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVenue(int id, UpdateVenueRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid venue update model.");

            var dto = new UpdateVenueDto
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Capacity = model.Capacity
            };

            await _venueService.UpdateVenue(id, dto);

            return Ok(new ServiceMessage { Success = true, Message = "Venue updated successfully." });
        }

        [HttpGet("sorted-by-capacity")]
        public async Task<IActionResult> GetVenuesSortedByCapacity(bool descending = false)
        {
            var venues = await _venueService.GetVenuesSortedByCapacity(descending);
            return Ok(new ServiceMessage<IEnumerable<VenueDto>> { Success = true, Data = venues });
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetVenuesByCity(City city)
        {
            var venues = await _venueService.GetVenuesByCity(city);
            return Ok(new ServiceMessage<IEnumerable<VenueDto>> { Success = true, Data = venues });
        }


        /// Search venues with pagination, optional name filter, and capacity sorting.

        [HttpGet("search")]
        public async Task<ServiceMessage<IEnumerable<VenueDto>>> SearchVenues(
            string nameStartsWith = null,
            int page = 1,
            int pageSize = 10,
            bool? sortByCapacityAsc = null,
            int? minCapacity = null
        )
        {
            return await _venueService.GetPagedVenues(nameStartsWith, page, pageSize, sortByCapacityAsc, minCapacity);
        }
    }
}

