using System;
using System.ComponentModel.DataAnnotations;

namespace TicketEase.WebApi.Models.Update
{
    public class PatchUserRequestModel
    {
        
        [StringLength(50, MinimumLength = 2)]
        public string? FirstName { get; set; }

        
        [StringLength(50, MinimumLength = 2)]
        public string? LastName { get; set; }

  
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
