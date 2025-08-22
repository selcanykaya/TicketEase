using System;
using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models
{
    public class PaymentRequestModel
    {
        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        public bool IsSuccessful { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
