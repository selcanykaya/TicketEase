using Microsoft.AspNetCore.Mvc;
using TicketEase.Business.Operations.Payment;
using TicketEase.Business.Operations.Payment.Dtos;
using TicketEase.WebApi.Models.Update;
using Microsoft.AspNetCore.Authorization;
using TicketEase.Business.Exceptions;
using TicketEase.WebApi.Filters;
using TicketEase.Business.Types;
using System.Collections.Generic;
using TicketEase.WebApi.Models;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPayment(PaymentRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid payment model.");

            var dto = new AddPaymentDto
            {
                Method = model.Method,
                IsSuccessful = model.IsSuccessful,
                OrderId = model.OrderId
            };

            await _paymentService.AddPayment(dto);

            var response = new ServiceMessage
            {
                Success = true,
                Message = "Payment recorded successfully."
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);

            var response = new ServiceMessage<PaymentDto>
            {
                Success = true,
                Message = "Payment retrieved successfully.",
                Data = payment
            };

            return Ok(response);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();

            var response = new ServiceMessage<IEnumerable<PaymentDto>>
            {
                Success = true,
                Message = "Payments retrieved successfully.",
                Data = payments
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentService.DeleteAsync(id);

            var response = new ServiceMessage
            {
                Success = true,
                Message = "Payment deleted successfully."
            };

            return Ok(response);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePayment(int id, UpdatePaymentRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid payment model.");

            var dto = new UpdatePaymentDto
            {
                Method = model.Method,
                TransactionId = model.TransactionId,
                PaymentDate = model.PaymentDate,
                IsSuccessful = model.IsSuccessful,
                OrderId = model.OrderId
            };

            await _paymentService.UpdatePayment(id, dto);

            var response = new ServiceMessage
            {
                Success = true,
                Message = "Payment updated successfully."
            };

            return Ok(response);
        }

        [HttpGet("exists/{transactionId}")]
        public async Task<IActionResult> PaymentExists(string transactionId)
        {
            var exists = await _paymentService.PaymentExists(transactionId);

            var response = new ServiceMessage<object>
            {
                Success = true,
                Message = "Payment existence checked successfully.",
                Data = new { TransactionId = transactionId, Exists = exists }
            };

            return Ok(response);
        }
    }
}
