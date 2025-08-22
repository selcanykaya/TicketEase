using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Event.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public EventType EventType { get; set; }

        public int VenueId { get; set; }
        public int OrganizerId { get; set; }
    }
}
