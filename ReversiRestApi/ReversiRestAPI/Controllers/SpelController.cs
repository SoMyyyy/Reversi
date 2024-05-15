using Microsoft.AspNetCore.Mvc;
using ReversiRestAPI;
using ReversiRestAPI.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReversieISpelImplementatie.Model;

[Route("api/Spel")]
[ApiController]
public class SpelController : ControllerBase
{
    // private readonly ReversiAPI_DBContext _context;
    private readonly ISpelRepository _contextSpelAccesLayerRepository;
    // private int _row;
    // private int _column;

    public SpelController(SpelAccessLayer contextSpelAccesLayerRepository)
    {
        _contextSpelAccesLayerRepository = contextSpelAccesLayerRepository;
    }


    // api/getspellen
    [HttpGet]
    public async Task<ActionResult> GetSpellenLijst()
    {
        var spellen = _contextSpelAccesLayerRepository.GetSpellen();
        // var spellen = _context.GetSpellen();
        // var spellenMetWachtendeSpeler = spellen.Where(s => string.IsNullOrEmpty(s.Speler2Token));
        // var omschrijvingen = spellenMetWachtendeSpeler.Select(s => s.Omschrijving);
        return Ok(spellen);
    }


    // GET api/spel
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<string>>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
    // {
    //     // var spellen = await _context.Spellen.ToListAsync();
    //     var spellen = _context.GetSpellen();
    //     var spellenMetWachtendeSpeler = spellen.Where(s => string.IsNullOrEmpty(s.Speler2Token));
    //     var omschrijvingen = spellenMetWachtendeSpeler.Select(s => s.Omschrijving);
    //     return Ok(omschrijvingen);
    // }

    // Post
    // api/spel
    [HttpPost]
    public async Task<IActionResult> PostSpel([FromBody] SpelInfoApi spelInfo)
    {
        // Create a new Spel instance
        Spel newSpel = new Spel
        {
            Speler1Token = spelInfo.SpelerToken,
            Omschrijving = spelInfo.SpelOmschrijving,
            Token = Guid.NewGuid()
                .ToString("N"), //// "N" to prevent hypens in the guid, which will not work nicely in REST. Because, reasons.
            // Speler2Token = "string.Empty", the second player can be Null so i can check later inside the Join action method in MVC, if the speler2Token != null then it means that the game can be joined
            AandeBeurt = Kleur.Wit,
            GameState = State.Wachten
        };

        if (newSpel == null)
        {
            return BadRequest("the Game u trying to make, must not be null");
        }

        // Add the new Spel to the context
        _contextSpelAccesLayerRepository.AddSpel(newSpel);

        // Return a success response
        return Ok(newSpel);
    }


    // Get api/spel/speltoken
    [HttpGet("GetSpelByToken/{token}")]
    public async Task<ActionResult<Spel>> GetSpelByToken(string token)
    {
        // var spel = await _context.Spellen.FirstOrDefaultAsync(s => s.Token == token);
        var spel = _contextSpelAccesLayerRepository.GetSpel(token);

        // if (spel.GameState == State.Klaar)
        // {
        //     // Return status code 204 when the game is finished
        //     return NoContent();
        // }

        if (spel == null)
        {
            return NotFound();
        }

        return Ok(spel);
    }


    [HttpGet("{id}")]
    public ActionResult<Spel> getSpelBySpelID(int id)
    {
        var spel = _contextSpelAccesLayerRepository.GetSpelById(id);
        // var spel = _context.Spellen.FirstOrDefault(s => s.ID == id);
        if (spel == null)
        {
            return NotFound();
        }

        return spel;
    }

    //! old
    //api/spel/spelerToken/{spelerToken}
    // [HttpGet("spelerToken/{spelerToken}")]
    // public async Task<ActionResult<Spel>> GetSpelBySpelerToken(string spelerToken)
    // {
    //     // public Spel GetSpelFromSpelerToken(string spelerToken)
    //     // {
    //     //     return _context.Spellen.FirstOrDefault(s => s.Speler1Token == spelerToken || s.Speler2Token == spelerToken);
    //     // }
    //
    //     var spel = _contextSpelAccesLayerRepository.GetSpelFromSpelerToken(spelerToken);
    //     if (spel.Speler2Token == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return spel;
    // }


    [HttpGet("spelerToken/{speler1Token}")]
    public async Task<ActionResult<Spel>> GetSpelBySpeler1Token(string speler1Token)
    {
        Console.WriteLine("Getting spel by speler token: {0}", speler1Token);

        Spel? spel = _contextSpelAccesLayerRepository.GetSpelFromSpeler1Token(speler1Token);

        var error = spel != null ? spel.ToString() : "There is no game associted to this player!";
        Console.WriteLine($"Retrieved spel {error}");
        if (spel == null)
        {
            Console.WriteLine($"Spel.Speler2Token is null for speler token:{speler1Token}");
            return NotFound();
        }

        if (spel.Speler1Token != speler1Token && spel.Speler2Token != null)
        {
            Console.WriteLine($"Spel is not null for speler token:{speler1Token} but the speler2Token is null and  he can join as speler2");
            return Ok(spel);
        }
        

        return spel;
    }
    
    
    
    [HttpGet("spelerToken/{speler2Token}")]
    public async Task<ActionResult<Spel>> GetSpelBySpeler2Token(string speler2Token)
    {
        Console.WriteLine("Getting spel by speler token: {0}", speler2Token);

        Spel? spel = _contextSpelAccesLayerRepository.GetSpelFromSpeler2Token(speler2Token);

        var error = spel != null ? spel.ToString() : "There is no game associted to this player!";
        Console.WriteLine($"Retrieved spel {error}");
        if (spel == null)
        {
            Console.WriteLine($"Spel is null for speler token:{speler2Token} and he is not associated with any game as joinPlayer");
            return NotFound();
        }

        
        

        return spel;
    }

    //! Endpoint to get the color of the player who is currently on turn
    [HttpGet("Beurt/{token}")]
    public async Task<ActionResult<Kleur>> GetBeurt(string token)
    {
        // var spel = await _context.Spellen.FirstOrDefaultAsync(s => s.Token == token);
        var spel = _contextSpelAccesLayerRepository.GetSpel(token);
        if (spel == null)
        {
            return NotFound();
        }

        return spel.AandeBeurt;
    }


    //Join after widgetFeedback 
    // [HttpPut("join/{token}")]
    [HttpPut("join/{token}")]
    public async Task<ActionResult<Spel>> JoinGame(string token, [FromBody] string playerWantsJoin)
    {
        // Get the existing Spel object from the database
        var existingSpel = _contextSpelAccesLayerRepository.GetSpel(token);

        // Check if the game can be joined
        if (existingSpel.Speler2Token != null)
        {
            return BadRequest("The game already has two players and can't be joined.");
        }

        // if (existingSpel.Speler1Token == playerWantsJoin)
        // {
        //     // The current user is already Speler1Token, they can't join as Speler2Token
        //     return BadRequest("You can't join the game as the second player because you are already the first player.");
        // }

        //! Determine if the current user is the game creator
        // bool isGameCreator = existingSpel.Speler1Token == playerWantsJoin;

        // Update the properties of the existing Spel object with the properties of the updated Spel object
        existingSpel.Speler2Token = playerWantsJoin;
        existingSpel.GameState = State.Bezig;

        // Save the changes to the database
        _contextSpelAccesLayerRepository.UpdateSpel(existingSpel);

        // Return the updated Spel object along with the IsGameCreator flag
        // return Ok(new { Spel = existingSpel, IsGameCreator = isGameCreator });
        return Ok(new { Spel = existingSpel });
    }


    //api/Spel/Zet
    [HttpPut("Zet")]
    public async Task<ActionResult<bool>> PutDoeZet([FromBody] ZetInfoApi zetInfoApi)
    {
        Spel spel = _contextSpelAccesLayerRepository.GetSpel(zetInfoApi.Speltoken);
        if (zetInfoApi.Speltoken == null)
        {
            return Unauthorized("SpelToken is leeg");
        }

        if ((spel.AandeBeurt == Kleur.Wit ? spel.Speler1Token : spel.Speler2Token) != zetInfoApi.SpelerToken)
        {
            return Unauthorized("Je speelt voor je beurt!");
        }

        if (zetInfoApi.Pass)
        {
            if (!spel.IsErEenZetMogelijk(spel.AandeBeurt))
            {
                spel.Pas();
                _contextSpelAccesLayerRepository.UpdateSpel(spel);
                return Content("Gepast");
            }
            else
            {
                return BadRequest("Passing is not allowed when there are valid moves.");
            }
        }

        if (spel.IsErEenZetMogelijk(spel.AandeBeurt) && !spel.Afgelopen())
        {
            if (spel.ZetMogelijk(zetInfoApi.RijZet, zetInfoApi.KolomZet) == true)
            {
                spel.DoeZet(zetInfoApi.RijZet, zetInfoApi.KolomZet);
                _contextSpelAccesLayerRepository.UpdateSpel(spel);
                if (spel.IsErEenZetMogelijk(Kleur.Zwart) == false && spel.IsErEenZetMogelijk(Kleur.Wit) == false)
                {
                    spel.GameState = State.Klaar;
                    _contextSpelAccesLayerRepository.UpdateSpel(spel);
                    //spel.Afgelopen();// return true 
                    return NoContent();
                    // return status 204 when the game is over
                }

                return true;
            }
            else
            {
                return BadRequest("Het zet is onmogelijk");
            }
        }


        Console.WriteLine("Geen zet mogleijk meer!");

        spel.GameState = State.Klaar;
        _contextSpelAccesLayerRepository.UpdateSpel(spel);
        return NoContent();
    }


    [HttpPut("pass")]
    public ActionResult<string> Pass([FromBody] PassModel passModel)
    {
        Spel spel = _contextSpelAccesLayerRepository.GetSpel(passModel.spelToken);
        
        // Check if spel is null and return NotFound status code
        if (spel == null)
        {
            return NotFound("Game not found.");
        }
        string playerColor = spel.AandeBeurt == Kleur.Wit ? "White" : "Black";
        if (spel.Speler1Token == passModel.spelerToken || spel.Speler2Token == passModel.spelerToken)
        {
            if (!spel.IsErEenZetMogelijk(spel.AandeBeurt))
            {
                spel.Pas();
                _contextSpelAccesLayerRepository.UpdateSpel(spel);
                return Content($"Passe is done for {playerColor}!");
            }
            else
            {
                return BadRequest("Passing is not allowed when there are valid moves.");
            }
        }

        return Unauthorized("Invalid player token.");
    }

    // PUT api/spel/opgeven
    [HttpPut("opgeven")]
    public ActionResult<string> GeefOp(string spelToken)
    {
        _contextSpelAccesLayerRepository.GetSpel(spelToken).Afgelopen();

        return Ok("Opgegeven, goed zo.");
    }


    [HttpDelete("/api/spel/verwijder/{spelToken}/")]
    public ActionResult<Spel> DeleteSpel(string spelToken)
    {
        Spel spel = _contextSpelAccesLayerRepository.GetSpel(spelToken);
        if (spel == null) return NotFound();

        _contextSpelAccesLayerRepository.VerwijderSpel(spel);
        return Ok(spel);
    }

    // //! Endpoint to give up
    // [HttpPut("Opgeven")]
    // public ActionResult<bool> PutOpgeven([FromBody] OpgevenInfoApi opgevenInfo)
    // {
    //     // var spel = await _context.Spellen.FirstOrDefaultAsync(s => s.Token == opgevenInfo.SpelToken);
    //     // var spel =  _context.GetSpel(opgevenInfo.SpelToken);
    //     // if (spel == null)
    //     // {
    //     //     return NotFound();
    //     // }
    //     //
    //     // if (spel.Speler1Token == opgevenInfo.SpelerToken || spel.Speler2Token == opgevenInfo.SpelerToken)
    //     // {
    //     //     // Logic to handle player giving up
    //     //     //! need to be implemented
    //     //     return Ok("Player has given up.");
    //     // }
    //     // else
    //     // {
    //     //     return BadRequest("The player token does not match any player in the game.");
    //     // }
    //     
    //     Spel spel = _context.GetSpel(opgevenInfo.SpelToken);
    //     if (spel == null || !opgevenInfo.Verify(spel)) return false;
    //
    //     spel.Opgeven();
    //     _context.UpdateSpel(spel);
    //     return true;
    // }
}

public class JoinGame
{
    public int GameId { get; set; }
    public string PlayerToken { get; set; }
}

public enum MoveResult
{
    Success,
    Failed,
    InvalidMove,
    NotYourTurn
}

public class AvailableGame
{
    public int ID { get; set; }
    public string Description { get; set; }
}

public class NewGame
{
    public string PlayerToken { get; set; }
    public string Description { get; set; }
}

public class SpelAuth
{
    public string SpelToken { get; set; }
    public string SpelerToken { get; set; }

    public bool Verify(Spel game)
    {
        if (game == null || (!game.Speler1Token.Equals(SpelerToken) && !game.Speler2Token.Equals(SpelerToken)))
        {
            return false;
        }

        return true;
    }
}

public class Move : SpelAuth
{
    public int MoveX { get; set; }
    public int MoveY { get; set; }
    public bool Pass { get; set; }
}