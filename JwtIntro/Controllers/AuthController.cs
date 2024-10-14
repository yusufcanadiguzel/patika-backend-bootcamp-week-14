using JwtIntro.Data;
using JwtIntro.Dtos;
using JwtIntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly IConfiguration _configuration;

        public AuthController(RepositoryContext repositoryContext, IConfiguration configuration)
        {
            _repositoryContext = repositoryContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Email = userRegisterDto.Email, Password = userRegisterDto.Password };

                _repositoryContext.Users.Add(user);

                await _repositoryContext.SaveChangesAsync();

                return Created();
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _repositoryContext.Users.Where(u => u.Email.Equals(userLoginDto.Email)).FirstOrDefaultAsync();

                if (user is null) return BadRequest();

                var token = Helper.CreateJwtToken(user.Email, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);

                return Ok(token);
            }

            return BadRequest();
        }
    }
}
