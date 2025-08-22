using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Event.Dtos;
using TicketEase.Business.Operations.User.Dtos;
using TicketEase.Business.Types;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Event
{
    public interface IEventService
    {
        Task<ServiceMessage> AddEvent(AddEventDto newEvent);

        Task<ServiceMessage> DeleteAsync(int id);

        Task<ServiceMessage<EventDto>> GetByIdAsync(int id);

        Task<ServiceMessage<IEnumerable<EventDto>>> GetAllAsync();

        Task<ServiceMessage> UpdateEvent(int id, UpdateEventDto dto);

        Task<ServiceMessage<IEnumerable<EventDto>>> GetPagedEvents(int page, int pageSize, EventType? eventType = null);

        Task<ServiceMessage<IEnumerable<EventDto>>> SearchEventsByName(string nameStartsWith);

        Task<IEnumerable<UserDto>> GetEventParticipantsAsync(int eventId);
    }
}
