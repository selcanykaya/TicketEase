using Microsoft.AspNetCore.Mvc;
using TicketEase.Business.Operations.Payment;
using TicketEase.Business.Operations.Payment.Dtos;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;
using Microsoft.AspNetCore.Authorization;
using TicketEase.Business.Exceptions;
using TicketEase.WebApi.Filters;

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
            return Ok(new { Message = "Payment recorded successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return Ok(payment);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentService.DeleteAsync(id);
            return NoContent();
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
            return Ok(new { Message = "Payment updated successfully." });
        }

        [HttpGet("exists/{transactionId}")]
        public async Task<IActionResult> PaymentExists(string transactionId)
        {
            var exists = await _paymentService.PaymentExists(transactionId);
            return Ok(new { TransactionId = transactionId, Exists = exists });
        }
    }
}
