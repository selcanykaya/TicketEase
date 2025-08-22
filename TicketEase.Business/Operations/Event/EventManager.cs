using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Event.Dtos;
using TicketEase.Business.Exceptions;
using TicketEase.Business.Types;
using TicketEase.Data.Entities;
using TicketEase.Data.Enums;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using TicketEase.Business.Operations.User.Dtos;

namespace TicketEase.Business.Operations.Event
{
    public class EventManager : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EventEntity> _eventRepository;

        public EventManager(IUnitOfWork unitOfWork, IRepository<EventEntity> eventRepository)
        {
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceMessage> AddEvent(AddEventDto newEvent)
        {
            var hasEvent = await _eventRepository.FindAsync(x => x.Name.ToLower() == newEvent.Name.ToLower());
            if (hasEvent.Any())
                throw new ConflictException("Event with this name already exists.");

            var eventEntity = new EventEntity
            {
                Name = newEvent.Name,
                Description = newEvent.Description,
                StartDate = newEvent.StartDate,
                EventType = newEvent.EventType,
                VenueId = newEvent.VenueId,
                OrganizerId = newEvent.OrganizerId
            };

            await _unitOfWork.BeginTransaction();
            await _eventRepository.AddAsync(eventEntity);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();

            return new ServiceMessage { Success = true, Message = "Event added successfully." };
        }

        public async Task<ServiceMessage> DeleteAsync(int id)
        {
            var entity = await _eventRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("Event not found.");

            await _unitOfWork.BeginTransaction();
            await _eventRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();

            return new ServiceMessage { Success = true, Message = "Event deleted successfully." };
        }

        public async Task<ServiceMessage<EventDto>> GetByIdAsync(int id)
        {
            var entity = await _eventRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("Event not found.");

            var dto = new EventDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EventType = entity.EventType,
                VenueId = entity.VenueId,
                OrganizerId = entity.OrganizerId
            };

            return new ServiceMessage<EventDto> { Success = true, Data = dto };
        }

        public async Task<ServiceMessage<IEnumerable<EventDto>>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            var dtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDate = e.StartDate,
                EventType = e.EventType,
                VenueId = e.VenueId,
                OrganizerId = e.OrganizerId
            });

            return new ServiceMessage<IEnumerable<EventDto>> { Success = true, Data = dtos };
        }

        public async Task<ServiceMessage> UpdateEvent(int id, UpdateEventDto dto)
        {
            var entity = await _eventRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("Event not found.");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.StartDate = dto.StartDate;
            entity.EventType = dto.EventType;
            entity.VenueId = dto.VenueId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransaction();
            await _eventRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();

            return new ServiceMessage { Success = true, Message = "Event updated successfully." };
        }

        public async Task<ServiceMessage<IEnumerable<EventDto>>> GetPagedEvents(int page, int pageSize, EventType? eventType = null)
        {
            Expression<Func<EventEntity, bool>> filter = null;
            if (eventType.HasValue) filter = e => e.EventType == eventType.Value;

            var events = await _eventRepository.GetPagedAsync(page, pageSize, filter, e => e.StartDate);

            if (!events.Any())
                throw new NotFoundException("No events found.");

            var dtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDate = e.StartDate,
                EventType = e.EventType,
                VenueId = e.VenueId,
                OrganizerId = e.OrganizerId
            });

            return new ServiceMessage<IEnumerable<EventDto>> { Success = true, Data = dtos };
        }

        public async Task<ServiceMessage<IEnumerable<EventDto>>> SearchEventsByName(string nameStartsWith)
        {
            if (string.IsNullOrWhiteSpace(nameStartsWith))
                return new ServiceMessage<IEnumerable<EventDto>> { Success = true, Data = new List<EventDto>() };

            var search = nameStartsWith.Trim().ToLower();
            var events = await _eventRepository.FindAsync(e => e.Name.ToLower().StartsWith(search));

            var dtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDate = e.StartDate,
                EventType = e.EventType,
                VenueId = e.VenueId,
                OrganizerId = e.OrganizerId
            });

            return new ServiceMessage<IEnumerable<EventDto>>
            {
                Success = true,
                Message = dtos.Any() ? "Events found." : "No events found.",
                Data = dtos
            };
        }

        public async Task<IEnumerable<UserDto>> GetEventParticipantsAsync(int eventId)
        {
            var users = await _eventRepository.Query()
                .Where(e => e.Id == eventId)
                .SelectMany(e => e.Tickets)
                .SelectMany(t => t.TicketOrders)
                .Select(to => to.Order.User)
                .Distinct()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .ToListAsync();

            return users;
        }





    }
}
