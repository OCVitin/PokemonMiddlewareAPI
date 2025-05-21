using Microsoft.AspNetCore.Mvc;
using Pokemon.Services;

namespace Pokemon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] GameRequest req)
        {
            var draw = _gameService.GetOrCreateDrawAsync(req.Date);
            return Ok(new { message = "Jogo iniciado!", date = req.Date.ToShortDateString() });
        }

        [HttpPost("guess")]
        public async Task<IActionResult> Guess([FromBody] GuessRequest req, [FromServices] GameService _gameService)
        {
            bool result = await _gameService.CheckGuessAsync(req.Nickname, req.Date, req.Guess);
            return Ok(new { correct = result });
        }

        [HttpGet("draw")]
        public async Task<IActionResult> GetDraw([FromQuery] DateTime date, [FromServices] GameService gameService)
        {
            var draw = await gameService.GetOrCreateDrawAsync(date);
            var pokemon = await gameService.GetPokemonByIdAsync(draw.PokemonId);

            if (pokemon == null)
                return NotFound("Pokémon não encontrado.");

            return Ok(new
            {
                Date = draw.Date.ToShortDateString(),
                Pokemon = new
                {
                    pokemon.Id,
                    pokemon.Name,
                    pokemon.PrimaryType,
                    pokemon.SecondaryType,
                    pokemon.SpriteUrl
                }
            });
        }
    }

    public class GameRequest
    {
        public required string Nickname { get; set; }
        public required string Plan { get; set; }
        public DateTime Date { get; set; }
    }

    public class GuessRequest
    {
        public required string Nickname { get; set; }
        public DateTime Date { get; set; }
        public required string Guess { get; set; }
    }
}
