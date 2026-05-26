using CrochetItAPI.Entities;

namespace CrochetItAPI.Services
{
    public interface IPatronService
    {
        Task<List<Patron>> GetAllPatronesAsync();
        Task<Patron?> GetPatronAsync(int ID);
        Task<Patron> CreatePatronAsync(Patron patron);
        Task<Patron> EditarPatronAsync(int ID, string nombre);
        Task<bool> DeletePatronAsync(int ID);
    }
}