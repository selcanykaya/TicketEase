using System.Collections.Generic;
using System.Threading.Tasks;
using TicketEase.Business.Operations.User.Dtos;
using TicketEase.Business.Types;
using TicketEase.Data.Enums;

namespace TicketEase.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user);
        Task<ServiceMessage<UserInfoDto>> LoginUser(LoginUserDto user);
        Task<ServiceMessage> DeleteAsync(int id);

        Task<ServiceMessage<UserDto>> GetByIdAsync(int id);
        Task<ServiceMessage<IEnumerable<UserDto>>> GetAllAsync();

        Task<ServiceMessage> UpdateUser(int id, UpdateUserDto dto);

        Task<ServiceMessage> UpdateUserTypeAsync(int id, UserType userType);
        Task<ServiceMessage> PatchUser(int id, PatchUserDto dto);

        Task<ServiceMessage<UserDto>> GetUserByEmailAsync(string email);
    }
}
