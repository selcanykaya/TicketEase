using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Payment.Dtos;

namespace TicketEase.Business.Operations.Payment
{
    public interface IPaymentService
    {
        Task AddPayment(AddPaymentDto addPaymentDto);                  
        Task DeleteAsync(int id);                                      
        Task<PaymentDto> GetByIdAsync(int id);                         
        Task<IEnumerable<PaymentDto>> GetAllAsync();                  
        Task UpdatePayment(int id, UpdatePaymentDto dto);              
        Task<bool> PaymentExists(string transactionId);                
    }
}
