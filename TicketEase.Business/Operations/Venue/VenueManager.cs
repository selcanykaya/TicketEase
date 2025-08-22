using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketEase.Business.Exceptions;
using TicketEase.Business.Operations.Venue.Dtos;
using TicketEase.Business.Types;
using TicketEase.Data.Entities;
using TicketEase.Data.Enums;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;

namespace TicketEase.Business.Operations.Venue
{
    public class VenueManager : IVenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<VenueEntity> _venueRepository;

        public VenueManager(IUnitOfWork unitOfWork, IRepository<VenueEntity> venueRepository)
        {
            _unitOfWork = unitOfWork;
            _venueRepository = venueRepository;
        }

        public async Task AddVenue(AddVenueDto venue)
        {
            var exists = await _venueRepository.ExistsAsync(x => x.Name.ToLower() == venue.Name.ToLower());
            if (exists)
                throw new ConflictException("Venue with this name already exists.");

            var venueEntity = new VenueEntity
            {
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City,
                Capacity = venue.Capacity
            };

            await _venueRepository.AddAsync(venueEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateVenue(int id, UpdateVenueDto dto)
        {
            var venueEntity = await _venueRepository.GetByIdAsync(id);
            if (venueEntity == null)
                throw new NotFoundException("Venue not found.");

            venueEntity.Name = dto.Name;
            venueEntity.Address = dto.Address;
            venueEntity.City = dto.City;
            venueEntity.Capacity = dto.Capacity;
            venueEntity.UpdatedAt = DateTime.UtcNow;

            await _venueRepository.UpdateAsync(venueEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var venueEntity = await _venueRepository.GetByIdAsync(id);
            if (venueEntity == null)
                throw new NotFoundException("Venue not found.");

            await _venueRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<VenueDto> GetByIdAsync(int id)
        {
            var entity = await _venueRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("Venue not found.");

            return new VenueDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                City = entity.City,
                Capacity = entity.Capacity
            };
        }

        public async Task<IEnumerable<VenueDto>> GetAllAsync()
        {
            var venues = await _venueRepository.GetAllAsync();
            return venues.Select(t => new VenueDto
            {
                Id = t.Id,
                Name = t.Name,
                Address = t.Address,
                City = t.City,
                Capacity = t.Capacity
            }).ToList();
        }

        public async Task<IEnumerable<VenueDto>> GetVenuesSortedByCapacity(bool descending = false)
        {
            var venues = await _venueRepository.GetSortedAsync(v => v.Capacity, descending);
            if (!venues.Any())
                throw new NotFoundException("No venues found.");

            return venues.Select(v => new VenueDto
            {
                Id = v.Id,
                Name = v.Name,
                Address = v.Address,
                City = v.City,
                Capacity = v.Capacity
            }).ToList();
        }

        public async Task<IEnumerable<VenueDto>> GetVenuesByCity(City city)
        {
            var venues = await _venueRepository.GetFilteredAsync(v => v.City == city);
            if (!venues.Any())
                throw new NotFoundException($"No venues found in {city}.");

            return venues.Select(v => new VenueDto
            {
                Id = v.Id,
                Name = v.Name,
                Address = v.Address,
                City = v.City,
                Capacity = v.Capacity
            }).ToList();
        }
    }
}
