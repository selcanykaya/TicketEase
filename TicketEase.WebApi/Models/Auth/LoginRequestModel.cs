using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TicketEase.WebApi.Models.Auth
{
    public class LoginRequestModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool RememberMe { get; set; } = false;
       
       

    }
}
