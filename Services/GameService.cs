using Pokemon.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Pokemon.Data

namespace Pokemon.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DailyDraw> GetOrCreateDrawAsync(DateTime date)
        {
            var existing = await _context.DailyDraws
                .FirstOrDefaultAsync(d => d.Date.Date == date.Date);

            if (existing != null) return existing;

            var last = await _context.DailyDraws
                .OrderByDescending(d => d.Date)
                .FirstOrDefaultAsync();

            var available = await _context.Pokemons
                .Where(p => last == null || p.Id != last.PokemonId)
                .ToListAsync();

            if (!available.Any())
                throw new InvalidOperationException("Nenhum Pokémon disponível para sorteio.");

            var rnd = new Random();
            var selected = available[rnd.Next(available.Count)];

            var draw = new DailyDraw
            {
                Date = date.Date,
                PokemonId = selected.Id
            };

            _context.DailyDraws.Add(draw);
            await _context.SaveChangesAsync();

            return draw;
        }

        public async Task<bool> CheckGuessAsync(string nickname, DateTime date, string guess)
        {
            var draw = await GetOrCreateDrawAsync(date);
            var pokemon = await _context.Pokemons.FindAsync(draw.PokemonId);
            return string.Equals(pokemon.Name, guess, StringComparison.OrdinalIgnoreCase);
        }
    }

}
