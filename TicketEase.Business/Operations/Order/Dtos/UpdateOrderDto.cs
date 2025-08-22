using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Order.Dtos
{
    public class UpdateOrderDto
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int UserId { get; set; }
        public List<int> TicketIds { get; set; } = new List<int>();
    }
}
