using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketEase.Business.Operations.User;
using TicketEase.Business.Operations.User.Dtos;
using TicketEase.Business.Exceptions;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Update;
using TicketEase.WebApi.Filters;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.Success)
                throw new NotFoundException(result.Message);

            return Ok(result);
        }

        [HttpGet("all")]
        [CacheFilter(120)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            if (!result.Success)
                throw new ValidationException(result.Message);

            return Ok(result);
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            if (!result.Success)
                throw new NotFoundException(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Success)
                throw new ValidationException(result.Message);

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid user model.");

            var dto = new UpdateUserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                UserType = model.UserType
            };

            var result = await _userService.UpdateUser(id, dto);
            if (!result.Success)
                throw new ValidationException(result.Message);

            return Ok(result);
        }

        [HttpPatch("update-type/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchUserType(int id, PatchUserTypeModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid user type model.");

            var result = await _userService.UpdateUserTypeAsync(id, model.UserType);
            if (!result.Success)
                throw new ValidationException(result.Message);

            return Ok(result);
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> PatchUser(int id, PatchUserRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Invalid user patch model.");

            var dto = new PatchUserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate
            };

            var result = await _userService.PatchUser(id, dto);
            if (!result.Success)
                throw new ValidationException(result.Message);

            return Ok(result);
        }
    }
}
