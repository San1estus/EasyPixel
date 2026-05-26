using CrochetItAPI.Entities;
using CrochetItAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CrochetItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronController : ControllerBase
    {
        private readonly IPatronService patronService;

        public PatronController(IPatronService patronService)
        {
            this.patronService = patronService;
        }
        [HttpGet("todos")]
        public async Task<ActionResult<List<Patron>>> GetAllPatrones()
        {
            var patrones = await patronService.GetAllPatronesAsync();
            if (patrones.Count == 0)
            {
                return NoContent();
            }
            return Ok(patrones);
        }
        // GET api/<PatronController>/5
        [HttpGet("obtenerpatron/{id}")]
        public async Task<ActionResult<Patron>> Get(int id)
        {
            var patron = await patronService.GetPatronAsync(id);
            if(patron == null)
            {
                return BadRequest("No se encontró el patrón");
            }
            return Ok(patron);
        }

        // POST api/<PatronController>
        [HttpPost("nuevopatron")]
        public async Task<ActionResult<Patron>> Post([FromBody] Patron patron)
        {
            var nuevoPatron = await patronService.CreatePatronAsync(patron);
            if (nuevoPatron == null)
            {
                return BadRequest("No se pudo registrar el patron.");
            }
            return Ok(patron);
        }

        // PUT api/<PatronController>/5
        [HttpPut("actualizarpatron/{id}")]
        public async Task<ActionResult<Patron>> Put(int id, [FromBody] string nombre)
        {
            var patronActualizado = await patronService.EditarPatronAsync(id, nombre);
            if(patronActualizado == null)
            {
                return BadRequest("No se pudo encontrar el patrón.");
            }
            return Ok(patronActualizado);
        }

        // DELETE api/<PatronController>/5
        [HttpDelete("eliminarpatron/{id}")]
        public async Task<ActionResult<Patron>> Delete(int id)
        {
            var patronEliminado = await patronService.DeletePatronAsync(id);
            if(!patronEliminado)
            {
                return BadRequest("No se pudo encontrar el patrón.");
            }
            return Ok("El patron se elimino.");
        }
    }
}