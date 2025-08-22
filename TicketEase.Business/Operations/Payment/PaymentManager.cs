using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketEase.Business.Operations.Payment.Dtos;
using TicketEase.Data.Entities;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;
using TicketEase.Business.Exceptions;

namespace TicketEase.Business.Operations.Payment
{
    public class PaymentManager : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PaymentEntity> _paymentRepository;

        public PaymentManager(IUnitOfWork unitOfWork, IRepository<PaymentEntity> paymentRepository)
        {
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
        }

        public async Task AddPayment(AddPaymentDto dto)
        {
            if (dto == null)
                throw new ValidationException("Payment data is required.");

            var payment = new PaymentEntity
            {
                OrderId = dto.OrderId,
                Method = dto.Method,
                IsSuccessful = dto.IsSuccessful,
                TransactionId = Guid.NewGuid().ToString("N").Substring(0, 25),
                PaymentDate = DateTime.Now
            };

            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException("Payment not found.");

            await _paymentRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException("Payment not found.");

            return new PaymentDto
            {
                Id = payment.Id,
                IsSuccessful = payment.IsSuccessful,
                Method = payment.Method,
                OrderId = payment.OrderId,
                TransactionId = payment.TransactionId,
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                IsSuccessful = p.IsSuccessful,
                Method = p.Method,
                OrderId = p.OrderId,
                TransactionId = p.TransactionId,
                PaymentDate = p.PaymentDate
            }).ToList();
        }

        public async Task UpdatePayment(int id, UpdatePaymentDto dto)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException("Payment not found.");

            payment.Method = dto.Method;
            payment.TransactionId = dto.TransactionId;
            payment.PaymentDate = dto.PaymentDate;
            payment.IsSuccessful = dto.IsSuccessful;
            payment.OrderId = dto.OrderId;
            payment.UpdatedAt = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> PaymentExists(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ValidationException("TransactionId is required.");

            return await _paymentRepository.ExistsAsync(p => p.TransactionId == transactionId);
        }
    }
}
