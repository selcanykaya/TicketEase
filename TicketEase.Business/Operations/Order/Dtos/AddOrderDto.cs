using System;
using System.Collections.Generic;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.Order.Dtos
{
    public class AddOrderDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        public List<int> TicketIds { get; set; } = new List<int>();
    }
}
