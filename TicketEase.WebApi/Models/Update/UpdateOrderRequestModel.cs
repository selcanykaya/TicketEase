using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models.Update
{
    public class UpdateOrderRequestModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public List<int> TicketIds { get; set; } = new List<int>();
    }
}
