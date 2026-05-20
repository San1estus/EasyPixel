using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CrochetIt.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
        }
        public async Task<bool> DeleteAsync(string endpoint, int id)
        {
            var response = await httpClient.DeleteAsync($"{endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await httpClient.GetAsync($"{endpoint}");

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public async Task<T> GetByIdAsync<T>(string endpoint, int id)
        {
            var response = await httpClient.GetAsync($"{endpoint}/{id}");

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result, options);
        }

        public async Task<T> PutAsync<T>(string endpoint, int id, object data)
        {
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
