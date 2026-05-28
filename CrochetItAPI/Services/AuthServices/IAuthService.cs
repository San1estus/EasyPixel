using CrochetItAPI.DTOs;

namespace CrochetItAPI.Services.AuthServices
{
    public interface IAuthService
    {
        Task<bool> CreateUser(NewUserDTO newUser);
        Task<TokenDTO> Login(UserDTO userDTO);
    }
}
