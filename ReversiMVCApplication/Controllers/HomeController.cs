using Microsoft.AspNetCore.Mvc;
using ReversiMVCApplication.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ReversiMVCApplication.Data;

namespace ReversiMVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReversiDbContext _context;
        

        public HomeController(ILogger<HomeController> logger, ReversiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // public async Task<IActionResult> Index()
        // {
        //     ClaimsPrincipal currentUser = this.User;
        //     var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //
        //     if (currentUserID != null)
        //     {
        //         var speler = await _context.Spelers.FindAsync(currentUserID);
        //         if (speler == null)
        //         {
        //             speler = new Speler { Guid = currentUserID, Naam = "Placeholder"};
        //             _context.Spelers.Add(speler);
        //             await _context.SaveChangesAsync();
        //         }
        //     }
        //
        //     return View();
        // }
        
        
        // public IActionResult Index()
        // {
        //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //
        //     var spel = _context.Spellen.FirstOrDefault(s => s.Speler1Token == userId || s.Speler2Token == userId);
        //
        //     if (spel != null)
        //     {
        //         // Redirect to the game page
        //         return RedirectToAction("Details", "Spellen", new { id = spel.SpelID });
        //     }
        //     else
        //     {
        //         // Redirect to the page where they can choose a game or create a new one
        //         return RedirectToAction("Index", "Spellen");
        //     }
        // }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if a Player exists in database.
            if (!_context.Spelers.Any(p => p.Guid == currentUserID))
            {
                Speler newPlayer = new Speler();
                newPlayer.Guid = currentUserID;
                newPlayer.Naam = currentUser.Identity.Name;
                _context.Spelers.Add(newPlayer);
                await _context.SaveChangesAsync();
            }

            // Check if the current player has a game running
            var spel = _context.Spel.FirstOrDefault(s => s.Speler1Token == currentUserID || s.Speler2Token == currentUserID);

            if (spel != null)
            {
                // Redirect to the game page
                return RedirectToAction("Details", "Spellen", new { id = spel.Id });
            }
            else
            {
                // Redirect to the page where they can choose a game or create a new one
                return RedirectToAction("Index", "Spellen");
            }
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
