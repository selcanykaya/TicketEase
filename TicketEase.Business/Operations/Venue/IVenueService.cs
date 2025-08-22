using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Venue.Dtos;
using TicketEase.Business.Types;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Venue
{
    public interface IVenueService
    {
        Task AddVenue(AddVenueDto addVenueDto); 
        Task DeleteAsync(int id);
        Task<VenueDto> GetByIdAsync(int id);
        Task<IEnumerable<VenueDto>> GetAllAsync();
        Task UpdateVenue(int id, UpdateVenueDto dto);
        Task<IEnumerable<VenueDto>> GetVenuesSortedByCapacity(bool descending = false);
        Task<IEnumerable<VenueDto>> GetVenuesByCity(City city);


        Task<ServiceMessage<IEnumerable<VenueDto>>> GetPagedVenues(
           string nameStartsWith = null,
           int page = 1,
           int pageSize = 10,
           bool? sortByCapacityAsc = null,
           int? minCapacity = null
       );
    }
}
