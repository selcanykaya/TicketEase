using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Ticket.Dtos;

namespace TicketEase.Business.Operations.Ticket
{
    public interface ITicketService
    {
        Task AddTicket(AddTicketDto ticketDto);                    
        Task DeleteAsync(int id);                                  
        Task<TicketDto> GetByIdAsync(int id);                     
        Task<IEnumerable<TicketDto>> GetAllAsync();                
        Task UpdateTicket(int id, UpdateTicketDto dto);           
        Task<bool> IsTicketSoldAsync(int ticketId);                
        Task<int> CountSoldTicketsByEventAsync(int eventId);      
    }
}
