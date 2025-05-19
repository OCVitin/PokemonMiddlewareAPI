namespace Pokemon.Models
{
    public class PokemonEntry
    {
        public int Id { get; set; } // ID no banco (opcional)
        public int PokeApiId { get; set; } // ID da PokéAPI
        public required string Name { get; set; }
        public required string PrimaryType { get; set; }
        public string? SecondaryType { get; set; }
        public required string SpriteUrl { get; set; }
    }
}
