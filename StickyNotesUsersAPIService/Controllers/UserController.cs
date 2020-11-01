using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StickyNotesUsersAPIService.Entities;
using StickyNotesUsersAPIService.Models;
using StickyNotesUsersAPIService.Repositories.Interfaces;

namespace StickyNotesUsersAPIService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        public UserController(IUserRepository repository, ILogger<UserController> logger, IConfiguration config)
        {
            userRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Users), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Users>> CreateUser([FromBody] Users user)
        {
            return await userRepository.Create(user);
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        [Authorize]
        public async Task<ActionResult<Users>> GetUserById(int userId)
        {
            var user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                _logger.LogError($"User with id: {userId}, hasn't been found in database.");
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("ValidateUserLogin")]
        public async Task<ActionResult<UserViewModel>> ValidateUserLogin([FromBody] UserModel model)
        {
            var user = await userRepository.ValidateUserLogin(model.Email, model.Password);
            if (user == null)
            {
                _logger.LogError($"User with id: {model.Email}, hasn't been found in database.");
                return NotFound();
            }
            else
            {
                var tokenString = BuildToken(user);
                return Ok(new UserViewModel { Email = user.Email, IsAuthorised = true, Token = tokenString, UserId = user.UserId, UserName = user.UserName });
            }
        }

        private string BuildToken(Users user)
        {
            var claims = new[] {
                new Claim (JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim (JwtRegisteredClaimNames.Email, user.Email),
                new Claim (JwtRegisteredClaimNames.Jti, user.UserId.ToString())
              };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
