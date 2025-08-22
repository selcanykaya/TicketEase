using System;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Payment.Dtos
{
    public class AddPaymentDto
    {
        public PaymentMethod Method { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public int OrderId { get; set; }
    }
}
