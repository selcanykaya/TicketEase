using System;
using System.ComponentModel.DataAnnotations;
using TicketEase.Data.Enums;

namespace TicketEase.WebApi.Models.Update
{
    public class UpdateUserRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Last name cannot start with a space.")]
        [System.ComponentModel.DefaultValue("Joe")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Last name cannot start with a space.")]
        [System.ComponentModel.DefaultValue("Doe")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public UserType UserType { get; set; }


    }
}
