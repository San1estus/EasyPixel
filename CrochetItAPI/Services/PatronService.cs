using CrochetItAPI.Data;
using CrochetItAPI.Entities;

namespace CrochetItAPI.Services
{
    public class PatronService : IPatronService
    {
        private readonly AppDbContext dbContext;

        public PatronService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Patron> CreatePatronAsync(Patron patron)
        {
            await dbContext.AddAsync(patron);
            var result = await dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return patron;
            }
            else
            {
                throw new Exception("No se pudo guardar el patron");
            }
        }

        public async Task<bool> DeletePatronAsync(int ID)
        {
            var busqueda = await dbContext.Patrones.FindAsync(ID);
            if (busqueda == null)
            {
                throw new Exception("No se encontró el patrón");
            }
            else
            {
                dbContext.Patrones.Remove(busqueda);
                var cambios = await dbContext.SaveChangesAsync();
                if (cambios > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("No se pudo actualizar el patron");
                }
            }
        }

        public async Task<Patron> EditarPatronAsync(int ID, string nombre)
        {
            var busqueda = await dbContext.Patrones.FindAsync(ID);
            if (busqueda == null)
            {
                throw new Exception("No se encontró el patrón");
            }
            else
            {
                busqueda.Nombre = nombre;
                dbContext.Patrones.Update(busqueda);
                var cambios = await dbContext.SaveChangesAsync();
                if (cambios > 0)
                {
                    return busqueda;
                }
                else
                {
                    throw new Exception("No se pudo actualizar el patron");
                }
            }
        }

        public async Task<Patron?> GetPatronAsync(int ID)
        {
            return await dbContext.Patrones.FindAsync(ID);
        }
    }
}