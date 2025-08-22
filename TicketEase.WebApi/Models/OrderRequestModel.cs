using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models
{
    public class OrderRequestModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        [Required]
        public List<int> TicketIds { get; set; } = new List<int>();
    }
}
