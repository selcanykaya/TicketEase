using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Order.Dtos;
using TicketEase.Business.Types;

namespace TicketEase.Business.Operations.Order
{
    public interface IOrderService
    {
        Task<ServiceMessage<int>> AddOrder(AddOrderDto addOrderDto);

        Task<ServiceMessage> DeleteAsync(int id);

        Task<ServiceMessage<OrderDto>> GetByIdAsync(int id);

        Task<ServiceMessage<IEnumerable<OrderDto>>> GetAllAsync();

        Task<ServiceMessage> UpdateOrder(int id, UpdateOrderDto dto);

        Task<ServiceMessage<IEnumerable<OrderDto>>> GetOrdersByUserIdAsync(int userId);
    }
}
