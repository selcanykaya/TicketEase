using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Payment.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public PaymentMethod Method { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }


        public int OrderId { get; set; }
    }
}
