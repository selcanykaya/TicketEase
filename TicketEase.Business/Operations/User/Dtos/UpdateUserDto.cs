using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.User.Dtos
{
    public class UpdateUserDto
    {
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public UserType UserType { get; set; }
    }
}
