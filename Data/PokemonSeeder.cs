using Pokemon.Models;
using System.Text.Json;
using System.Net.Http.Json;

namespace Pokemon.Data
{
    public class PokemonSeeder
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public PokemonSeeder(AppDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
        }

        public async Task SeedFirst151Async()
        {
            if (_context.Pokemons.Any()) return; // já populado

            for (int i = 1; i <= 151; i++)
            {
                var response = await _httpClient.GetFromJsonAsync<JsonElement>($"pokemon/{i}");

                var name = response.GetProperty("name").GetString();
                var typesArray = response.GetProperty("types").EnumerateArray().ToList();

                var primaryType = typesArray[0].GetProperty("type").GetProperty("name").GetString();
                string? secondaryType = typesArray.Count > 1
                    ? typesArray[1].GetProperty("type").GetProperty("name").GetString()
                    : null;

                var spriteUrl = response.GetProperty("sprites").GetProperty("front_default").GetString();

                var pokemon = new PokemonEntry
                {
                    PokeApiId = i,
                    Name = name!,
                    PrimaryType = primaryType!,
                    SecondaryType = secondaryType,
                    SpriteUrl = spriteUrl!
                };

                _context.Pokemons.Add(pokemon);

                Console.WriteLine($"Adicionado: {i} - {name}");
                await Task.Delay(100); // boa prática para não sobrecarregar a API
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Pokémons inseridos com sucesso!");
        }
    }
}
