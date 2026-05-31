using CrochetIt.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrochetIt.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IApiService apiService;

        public AuthService(IApiService apiService)
        {
            this.apiService = apiService;
        }
        public async Task<(string userId, string userName)> GetUserInfoAsync()
        {
            var userId = await SecureStorage.Default.GetAsync("user_id");
            var userName = await SecureStorage.Default.GetAsync("user_name");
            return (userId, userName);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");
            var expirationStr = await SecureStorage.Default.GetAsync("token_expiration");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expirationStr))
                return false;

            if (!DateTime.TryParse(expirationStr, out var expiration))
                return false;

            return DateTime.UtcNow < expiration;
        }

        public async Task<TokenResponse> LoginAsync(string email, string password)
        {
            try
            {
                var result = await apiService.PostAsync<TokenResponse>("auth/login", new { Email=email, Password=password });

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await SecureStorage.Default.SetAsync("auth_token", result.Token);
                    await SecureStorage.Default.SetAsync("token_expiration", result.Expiration.ToString("O"));
                    await SecureStorage.Default.SetAsync("user_name", result.UserName);
                    await SecureStorage.Default.SetAsync("user_id", result.UserId);
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error durante el login: " + ex.Message);
            }
        }

        public Task LogoutAsync()
        {
            SecureStorage.Default.Remove("auth_token");
            SecureStorage.Default.Remove("token_expiration");
            return Task.CompletedTask;
        }

        public async Task<bool> RegisterAsync(string email, string userName, string password)
        {
            try
            {
                return await apiService.PostVoidAsync("auth/register",
                    new { email, userName, password });
            }
            catch (Exception ex)
            {
                throw new Exception("Error durante el registro: " + ex.Message);
            }
        }
    }
}