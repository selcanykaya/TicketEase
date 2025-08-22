using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models.Update
{
    public class UpdateVenueRequestModel
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Address { get; set; }

        [Required]
        public City City { get; set; }

        [Required]
        public int Capacity { get; set; }
    }
}
