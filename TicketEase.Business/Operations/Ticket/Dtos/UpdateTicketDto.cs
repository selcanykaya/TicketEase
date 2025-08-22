using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketEase.Business.Operations.Ticket.Dtos
{
    public class UpdateTicketDto
    {
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }

        public int EventId { get; set; }

    }
}
