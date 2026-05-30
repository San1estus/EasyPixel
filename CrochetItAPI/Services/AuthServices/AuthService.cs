using CrochetItAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrochetItAPI.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        public async Task<bool> CreateUser(NewUserDTO newUser)
        {
            var newIdentityUser = new IdentityUser
            {
                Email = newUser.email,
                UserName = newUser.userName
            };

            var result = await userManager.CreateAsync(newIdentityUser, newUser.password);

            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenDTO> Login(UserDTO userDTO)
        {
            var result = await signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, false, false);
            if (result.Succeeded)
            {
                var userId = await userManager.FindByEmailAsync(userDTO.Email);
                return BuildToken(userId.UserName, userId.Id);
            }

            return new TokenDTO();
        }

        private TokenDTO BuildToken(string userName, string id)
        {
            var claims = new List<Claim>()
            {
                new Claim("User", userName),
                new Claim("Expiracion", "1 hora"),
                new Claim(ClaimTypes.NameIdentifier, id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var expiration = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new TokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}