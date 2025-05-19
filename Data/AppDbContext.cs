using Pokemon.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pokemon.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<DailyDraw> DailyDraws { get; set; }
        public DbSet<PokemonEntry> Pokemons { get; set; }
    }
}
