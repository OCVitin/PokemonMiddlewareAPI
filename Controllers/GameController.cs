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
            var draw = _gameService.GetOrCreateDraw(req.Date);
            return Ok(new { message = "Jogo iniciado!", date = req.Date.ToShortDateString() });
        }

        [HttpPost("guess")]
        public IActionResult Guess([FromBody] GuessRequest req)
        {
            bool result = _gameService.CheckGuess(req.Nickname, req.Date, req.Guess);
            return Ok(new { correct = result });
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
