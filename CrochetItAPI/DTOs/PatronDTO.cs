namespace CrochetItAPI.DTOs
{
    public class PatronDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
