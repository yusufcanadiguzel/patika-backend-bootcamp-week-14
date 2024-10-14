using IdentityIntro.Data;
using IdentityIntro.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, RepositoryContext repositoryContext, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _repositoryContext = repositoryContext;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };

                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (result.Succeeded)
                {
                    user.Password = identityUser.PasswordHash;

                    _repositoryContext.Users.Add(user);

                    await _repositoryContext.SaveChangesAsync();

                    return Ok(new { Message = "User registered." });
                }
            }

            return BadRequest(new { Errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);

                if (result.Succeeded)
                    return Ok(new { Message = "Login successfull." });
                else
                    return Unauthorized(new { Message = "Mail or password error." });
            }

            return BadRequest(new { Messages = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });
        }
    }
}
