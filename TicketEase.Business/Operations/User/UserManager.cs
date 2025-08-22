using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketEase.Business.Operations.User.Dtos;
using TicketEase.Business.Types;
using TicketEase.Data.Entities;
using TicketEase.Data.Enums;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;
using TicketEase.Business.Exceptions;

namespace TicketEase.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ValidationException("Email is required.");

            var existUser = await _userRepository.ExistsAsync(x => x.Email.ToLower() == user.Email.ToLower());
            if (existUser)
                throw new ConflictException("User with this email already exists.");

            var userEntity = new UserEntity
            {
                Email = user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                UserType = UserType.Customer
            };

            await _userRepository.AddAsync(userEntity);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { Success = true, Message = "User registered successfully." };
        }

        public async Task<ServiceMessage<UserInfoDto>> LoginUser(LoginUserDto user)
        {
            var userEntity = await _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            
            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(user.Password, userEntity.PasswordHash))
            {
                return new ServiceMessage<UserInfoDto>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var userInfo = new UserInfoDto
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                UserType = userEntity.UserType
            };

            return new ServiceMessage<UserInfoDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = userInfo
            };
        }


        public async Task<ServiceMessage> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            user.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { Success = true, Message = "User deleted successfully." };
        }

        public async Task<ServiceMessage<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            return new ServiceMessage<UserDto>
            {
                Success = true,
                Data = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthDate = user.BirthDate,
                    UserType = user.UserType
                }
            };
        }

        public async Task<ServiceMessage<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(t => new UserDto
            {
                Id = t.Id,
                Email = t.Email,
                FirstName = t.FirstName,
                LastName = t.LastName,
                BirthDate = t.BirthDate,
                UserType = t.UserType
            }).ToList();

            return new ServiceMessage<IEnumerable<UserDto>> { Success = true, Message = "Users fetched successfully.", Data = userDtos };
        }

        public async Task<ServiceMessage> UpdateUser(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.BirthDate = dto.BirthDate;
            user.UserType = dto.UserType;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { Success = true, Message = "User updated successfully." };
        }

        public async Task<ServiceMessage> UpdateUserTypeAsync(int id, UserType userType)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            user.UserType = userType;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { Success = true, Message = "User type updated successfully." };
        }

        public async Task<ServiceMessage> PatchUser(int id, PatchUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (!string.IsNullOrEmpty(dto.FirstName))
                user.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName))
                user.LastName = dto.LastName;
            if (dto.BirthDate.HasValue)
                user.BirthDate = dto.BirthDate.Value;

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage { Success = true, Message = "User updated successfully." };
        }

        public async Task<ServiceMessage<UserDto>> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email cannot be empty.");

            var user = await _userRepository.Get(u => u.Email == email);
            if (user == null)
                throw new NotFoundException("User not found.");

            var dto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                UserType = user.UserType
            };

            return new ServiceMessage<UserDto> { Success = true, Data = dto };
        }
    }
}
