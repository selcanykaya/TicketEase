using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketEase.Business.Operations.User;
using TicketEase.Business.Operations.User.Dtos;
using TicketEase.WebApi.Jwt;
using TicketEase.WebApi.Models;
using TicketEase.WebApi.Models.Auth;

namespace TicketEase.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.Success)
                return Ok(new { Message = result.Message });

            return BadRequest(new { Message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginUser(new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe
            });

            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            var user = result.Data;

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = _configuration["Jwt:SecretKey"],
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                ExpireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]),
                RememberMe = request.RememberMe
            });

            return Ok(new LoginResponseModel
            {
                Message = "Login successful.",
                Token = token
            });
        }
    }
}
