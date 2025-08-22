using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models.Update
{
    public class UpdateEventRequestModel
    {
        [Required]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Event name cannot start with a space.")]
        [System.ComponentModel.DefaultValue("New Event")]
        [StringLength(100, MinimumLength = 6)]
        public string Name { get; set; }

        [StringLength(1000, MinimumLength = 0)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        public EventType EventType { get; set; }

        [Required]
        public int VenueId { get; set; }
    }
}
