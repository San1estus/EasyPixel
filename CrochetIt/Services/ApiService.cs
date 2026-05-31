using CrochetIt.Services.AuthServices;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CrochetIt.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;
        private readonly IAuthService authService;
        private readonly JsonSerializerOptions options;

        public ApiService(HttpClient httpClient, IAuthService authService)
        {
            this.httpClient = httpClient;
            this.authService = authService;
            this.options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
        }
        private async Task AddAuthorizationHeader()
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<bool> DeleteAsync(string endpoint, int id)
        {
            await AddAuthorizationHeader();
            var response = await httpClient.DeleteAsync($"{endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            await AddAuthorizationHeader();
            var response = await httpClient.GetAsync($"{endpoint}");

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public async Task<T> GetByIdAsync<T>(string endpoint, int id)
        {
            await AddAuthorizationHeader();
            var response = await httpClient.GetAsync($"{endpoint}/{id}");

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            await AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var result = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<T>(result, options);
            }
            catch (JsonException)
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)result;
                }

                return default;
            }
        }

        public async Task<T> PutAsync<T>(string endpoint, int id, object data)
        {
            await AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{endpoint}/{id}", content);

            if (!response.IsSuccessStatusCode)
                return default;

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result, options);
        }
    }
}
