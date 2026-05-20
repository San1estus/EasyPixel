using System;
using System.Collections.Generic;
using System.Text;

namespace CrochetIt.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> GetByIdAsync<T>(string endpoint, int id);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<T> PutAsync<T>(string endpoint, int id, object data);
        Task<bool> DeleteAsync(string endpoint, int id);
    }
}
