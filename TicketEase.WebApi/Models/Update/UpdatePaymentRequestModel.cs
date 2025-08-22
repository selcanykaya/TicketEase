using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models.Update
{
    public class UpdatePaymentRequestModel
    {
        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        public bool IsSuccessful { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Transaction ID must be between 1 and 25 characters.")]
        public string TransactionId { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime PaymentDate { get; set; }

    }
}
