using System.ComponentModel.DataAnnotations;

namespace CrochetItAPI.DTOs
{
    public class UserDTO
    {
        [EmailAddress(ErrorMessage = "El formato de correo es inválido.")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
