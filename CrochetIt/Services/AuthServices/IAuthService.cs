using CrochetIt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrochetIt.Services.AuthServices
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string email, string username, string password);
        Task<TokenResponse> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<(string userId, string userName)> GetUserInfoAsync();
    }
}
