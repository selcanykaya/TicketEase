using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Ticket.Dtos;
using TicketEase.Data.Entities;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;
using TicketEase.Business.Exceptions; 

namespace TicketEase.Business.Operations.Ticket
{
    public class TicketManager : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TicketEntity> _ticketRepository;

        public TicketManager(IUnitOfWork unitOfWork, IRepository<TicketEntity> ticketRepository)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
        }

        public async Task AddTicket(AddTicketDto ticket)
        {
            var exists = await _ticketRepository.ExistsAsync(
                x => x.EventId == ticket.EventId && x.SeatNumber == ticket.SeatNumber
            );

            if (exists)
                throw new ConflictException("This seat is already assigned for the selected event.");

            var ticketEntity = new TicketEntity
            {
                EventId = ticket.EventId,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price,
                IsSold = ticket.IsSold
            };

            await _ticketRepository.AddAsync(ticketEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                throw new NotFoundException("Ticket not found.");

            await _ticketRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TicketDto> GetByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                throw new NotFoundException("Ticket not found.");

            return new TicketDto
            {
                Id = ticket.Id,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price,
                IsSold = ticket.IsSold,
                EventId = ticket.EventId
            };
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();

            return tickets.Select(t => new TicketDto
            {
                Id = t.Id,
                SeatNumber = t.SeatNumber,
                Price = t.Price,
                IsSold = t.IsSold,
                EventId = t.EventId
            }).ToList();
        }

        public async Task UpdateTicket(int id, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                throw new NotFoundException("Ticket not found.");

            ticket.SeatNumber = dto.SeatNumber;
            ticket.Price = dto.Price;
            ticket.IsSold = dto.IsSold;
            ticket.EventId = dto.EventId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsTicketSoldAsync(int ticketId)
        {
            var isSold = await _ticketRepository.ExistsAsync(t => t.Id == ticketId && t.IsSold);
            return isSold;
        }

        public async Task<int> CountSoldTicketsByEventAsync(int eventId)
        {
            var soldCount = await _ticketRepository.CountAsync(t => t.EventId == eventId && t.IsSold);
            return soldCount;
        }
    }
}
