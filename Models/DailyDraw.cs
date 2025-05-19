namespace Pokemon.Models
{
    public class DailyDraw
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PokemonId { get; set; }
    }
}
