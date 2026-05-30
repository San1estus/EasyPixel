using CrochetItAPI.DTOs;
using CrochetItAPI.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;


namespace CrochetItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] NewUserDTO newUser)
        {
            var result = await authService.CreateUser(newUser);
            if (result)
            {
                var loginResult = await authService.Login(new UserDTO { Email = newUser.email, Password = newUser.password });
                return Ok(loginResult);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var result = await authService.Login(userDTO);
            return Ok(result);
        }
    }
}
