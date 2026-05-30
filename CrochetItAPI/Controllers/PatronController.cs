using CrochetItAPI.DTOs;
using CrochetItAPI.Entities;
using CrochetItAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CrochetItAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet("obtenerpatron/{id}")]
        public async Task<ActionResult<PatronDTO>> Get(int id)
        {
            var patron = await patronService.GetPatronAsync(id);
            if (patron == null)
            {
                return BadRequest("No se encontró el patrón");
            }

            // Mapear a DTO
            var patronDTO = new PatronDTO
            {
                Id = patron.Id,
                Nombre = patron.Nombre,
                ImageUrl = patron.ImageUrl,
                UserId = patron.UserId,
                UserName = patron.UserName
            };

            return Ok(patronDTO);
        }

        [HttpPost("nuevopatron")]
        public async Task<ActionResult<Patron>> Post([FromBody] Patron patron)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst("User")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("No hay usuario autenticado");
            }

            patron.UserId = userId;
            patron.UserName = userName;

            var nuevoPatron = await patronService.CreatePatronAsync(patron);
            if (nuevoPatron == null)
            {
                return BadRequest("No se pudo registrar el patron.");
            }
            return Ok(patron);
        }

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