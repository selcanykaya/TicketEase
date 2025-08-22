using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketEase.Business.Operations.Order.Dtos;
using TicketEase.Business.Types;
using TicketEase.Data.Entities;
using TicketEase.Data.Enums;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;

namespace TicketEase.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<TicketEntity> _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(
            IRepository<OrderEntity> orderRepository,
            IRepository<TicketEntity> ticketRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceMessage<int>> AddOrder(AddOrderDto addOrderDto)
        {
            try
            {
                var tickets = (await _ticketRepository.FindAsync(t => addOrderDto.TicketIds.Contains(t.Id))).ToList();
                if (tickets.Count != addOrderDto.TicketIds.Count)
                    return new ServiceMessage<int> { Success = false, Message = "Some tickets do not exist." };

                if (tickets.Any(t => t.IsSold))
                    return new ServiceMessage<int> { Success = false, Message = "One or more tickets are already sold." };

                var totalAmount = tickets.Sum(t => t.Price);

                var orderEntity = new OrderEntity
                {
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    UserId = addOrderDto.UserId,
                    TicketOrders = tickets.Select(t => new TicketOrderEntity { TicketId = t.Id }).ToList()
                };

                foreach (var ticket in tickets) ticket.IsSold = true;

                await _orderRepository.AddAsync(orderEntity);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage<int>
                {
                    Success = true,
                    Message = "Order added successfully.",
                    Data = orderEntity.Id
                };
            }
            catch (Exception ex)
            {
                return new ServiceMessage<int>
                {
                    Success = false,
                    Message = $"Error while adding order: {ex.Message}"
                };
            }
        }

        public async Task<ServiceMessage> DeleteAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                    return new ServiceMessage { Success = false, Message = "Order not found." };

                await _orderRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage { Success = true, Message = "Order deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceMessage { Success = false, Message = $"Error while deleting order: {ex.Message}" };
            }
        }

        public async Task<ServiceMessage<OrderDto>> GetByIdAsync(int id)
        {
            var entity = await _orderRepository.Query()
                .Include(o => o.TicketOrders)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (entity == null)
                return new ServiceMessage<OrderDto> { Success = false, Message = "Order not found." };

            var dto = new OrderDto
            {
                Id = entity.Id,
                OrderDate = entity.OrderDate,
                TotalAmount = entity.TotalAmount,
                Status = entity.Status,
                UserId = entity.UserId,
                TicketIds = entity.TicketOrders?.Select(to => to.TicketId).ToList() ?? new List<int>()
            };

            return new ServiceMessage<OrderDto> { Success = true, Data = dto };
        }

        public async Task<ServiceMessage<IEnumerable<OrderDto>>> GetAllAsync()
        {
            var orders = await _orderRepository.Query()
                .Include(o => o.TicketOrders)
                .ToListAsync();

            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserId = o.UserId,
                TicketIds = o.TicketOrders?.Select(to => to.TicketId).ToList() ?? new List<int>()
            }).ToList();

            return new ServiceMessage<IEnumerable<OrderDto>> { Success = true, Message = "Orders fetched successfully.", Data = dtos };
        }

        public async Task<ServiceMessage> UpdateOrder(int id, UpdateOrderDto dto)
        {
            try
            {
                var orderEntity = await _orderRepository.Query()
                    .Include(o => o.TicketOrders)
                    .ThenInclude(to => to.Ticket)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (orderEntity == null)
                    return new ServiceMessage { Success = false, Message = "Order not found." };

                foreach (var oldTicketOrder in orderEntity.TicketOrders)
                    if (oldTicketOrder.Ticket != null) oldTicketOrder.Ticket.IsSold = false;

                var newTickets = await _ticketRepository.FindAsync(t => dto.TicketIds.Contains(t.Id));
                foreach (var ticket in newTickets) ticket.IsSold = true;

                orderEntity.TicketOrders.Clear();
                orderEntity.TicketOrders = newTickets.Select(t => new TicketOrderEntity { TicketId = t.Id }).ToList();

                orderEntity.TotalAmount = newTickets.Sum(t => t.Price);
                orderEntity.OrderDate = dto.OrderDate;
                orderEntity.Status = dto.Status;
                orderEntity.UserId = dto.UserId;
                orderEntity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage { Success = true, Message = "Order updated successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceMessage { Success = false, Message = $"Error while updating order: {ex.Message}" };
            }
        }

        public async Task<ServiceMessage<IEnumerable<OrderDto>>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.Query(o => o.UserId == userId)
                                               .Include(o => o.TicketOrders)
                                               .ToListAsync();

            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserId = o.UserId,
                TicketIds = o.TicketOrders?.Select(to => to.TicketId).ToList() ?? new List<int>()
            }).ToList();

            return new ServiceMessage<IEnumerable<OrderDto>> { Success = true, Message = "Orders fetched successfully.", Data = dtos };
        }
    }
}
