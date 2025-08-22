using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models
{
    public class PatchUserTypeModel
    {
        [Required]
        public UserType UserType { get; set; }
    }
}
