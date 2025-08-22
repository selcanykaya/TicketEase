using System.ComponentModel.DataAnnotations;

namespace TicketEase.WebApi.Models
{
    public class TicketRequestModel
    {
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string SeatNumber { get; set; }

        [Required]
        [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public bool IsSold { get; set; } = false;

        [Required]
        public int EventId { get; set; } 
    }
}
