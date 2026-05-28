using SkiaSharp;

namespace CrochetItAPI.Entities
{
    public class Patron
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
