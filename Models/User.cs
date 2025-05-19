namespace Pokemon.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Nickname { get; set; }
        public required string Plan { get; set; } // "basic" ou "premium"
    }
}
