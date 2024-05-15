using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReversiMVCApplication.Data;
using ReversiMVCApplication.Models;
using ReversiMVCApplication.Services;

namespace ReversiMVCApplication.Controllers
{
    [Authorize]
    public class SpellenController : Controller
    {
        private readonly ReversiDbContext _context;
        private readonly ApiService _apiService;
        private readonly ILogger<SpellenController> _logger;
        private readonly UserManager<IdentityUser> _userManager;


        public SpellenController(ReversiDbContext context, ApiService apiService, ILogger<SpellenController> logger,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _apiService = apiService;
            _logger = logger;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            List<Spel> spelInfo = await _apiService.GetSpelInfo();

            // Create a new list to hold the view models
            List<SpelViewModel> spelViewModels = new List<SpelViewModel>();

            foreach (var s in spelInfo.Where(s => s.GameState == 0))
            {
                var speler1 = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == s.Speler1Token);
                IdentityUser? speler2 = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == s.Speler2Token);

                Console.WriteLine("This is speler1 =" + speler1);
                Console.WriteLine("This is speler2 =" + speler2);
                // Create a new view model and add it to the list
                spelViewModels.Add(new SpelViewModel
                {
                    Speler1UserName = speler1?.UserName,
                    Speler2UserName = speler2?.UserName,
                    Omschrijving = s.Omschrijving,
                    AanDeBeurt = s.AanDeBeurt,
                    GameState = s.GameState,
                    Id = s.Id,
                    Token = s.Token
                });
            }

            return View(spelViewModels);
        }

        public async Task<IActionResult> BusyGames()
        {
            List<Spel> spelInfo = await _apiService.GetSpelInfo();

            // Create a new list to hold the view models
            List<SpelViewModel> spelViewModels = new List<SpelViewModel>();

            // current user 
            var currentUserToken = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            foreach (var s in spelInfo.Where(s =>
                         (s.GameState == 1) &&
                         (s.Speler1Token == currentUserToken || s.Speler2Token == currentUserToken)))
            {
                var speler1 = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == s.Speler1Token);
                IdentityUser? speler2 = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == s.Speler2Token);

                Console.WriteLine("This is speler1 =" + speler1);
                Console.WriteLine("This is speler2 =" + speler2);
                // Create a new view model and add it to the list
                spelViewModels.Add(new SpelViewModel
                {
                    Speler1UserName = speler1?.UserName,
                    Speler2UserName = speler2?.UserName,
                    Omschrijving = s.Omschrijving,
                    AanDeBeurt = s.AanDeBeurt,
                    GameState = s.GameState,
                    Id = s.Id,
                    Token = s.Token
                });
            }

            return View(spelViewModels);
        }


        public async Task<IActionResult> Join(string token)
        {
            Console.WriteLine("This is the passed token " + token);

            if (token == null)
            {
                return RedirectToAction("Index");
            }

            // Retrieve the current user's token
            var currentUserToken = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(currentUserToken);

            // Call the GetSpelByToken method from the ApiService
            var spel = await _apiService.GetSpelByToken(token);

            if (currentUserToken == null)
            {
                // Handle the case where the user is not authenticated
                TempData["Error"] = "The user try's to join is Null";
                return RedirectToAction("Index");
                // // Handle the case where the user is not authenticated
                // return BadRequest("The user try's to join is Null");
            }

            if (spel.Token == null)
            {
                // If the token is invalid, redirect back to Index
                Console.WriteLine("Invalid Token has been passed, the passed token = " + token);
                TempData["Error"] = "Invalid Token has been passed!";
                return RedirectToAction("Index");
            }

            // if the user who try to join is the speler1Token||Speler2Token, which is always the user who created the game and who already in the game
            // then redirect to the spelen and give the token to show the game 
            if (currentUserToken == spel.Speler1Token || currentUserToken == spel.Speler2Token)
            {
                return RedirectToAction("Spelen", new { token = spel.Token });
            }


            //! Checking if the player is already associated with a game
            var existingSpel = await _apiService.GetSpelFromSpeler2Token(currentUserToken);
            // var existingSpel = await _apiService.GetSpelByToken(token);
            if (existingSpel.Speler2Token != null)
            {
                // Show a message that they need to finish their current game first
                TempData["Error"] = "Je zit al in een spel! Ga naar 'Continue Your Game!'";
                // TempData["Error"] = "Je zit al in een spel! Ga naar 'Continue Your Game!'";
                return RedirectToAction("Index");
            }

            //if speler 2 is null 


            var joinRespond = await _apiService.JoinGame(token, currentUserToken);


            Console.WriteLine(joinRespond);
            // If everything goes well, redirect the user to the game view
            return RedirectToAction("Spelen", new { token = spel.Token });
        }


        public async Task<IActionResult> Spelen(string token)
        {
            if (token == null)
            {
                // return new ObjectResult(new { message = "Gebruik de knoppen om bij een spel te komen!" }) { StatusCode = StatusCodes.Status403Forbidden };

                TempData["Message"] = "Gebruik de knoppen om bij een spel te komen!";
                return RedirectToAction("Index");
            }

            var spel = await _apiService.GetSpelByToken(token);

            return View(spel);
        }


  

        
        public async Task<IActionResult> Pass(string spelToken)
        {
            Console.WriteLine("This is the passed token " + spelToken);

            // Retrieve the current user's token
            var currentUserToken = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("This is the CurrentUserId = "+currentUserToken);

            if (currentUserToken == null)
            {
                // Handle the case where the user is not authenticated
                TempData["Message"] = "currentUser is null en kan niet passen!";
                return RedirectToAction("Spelen");
                return Json(new { success = false, message = "currentUser is null en kan niet passen" });
            }

            // Call the GetSpelByToken method from the ApiService
            var spel = await _apiService.GetSpelByToken(spelToken);

            if (spel == null)
            {
                TempData["Message"] = "spel of spelerToken zijn null";
                return RedirectToAction("Spelen");
            }

            try
            {
                var passRequest = await _apiService.Pass(spelToken, currentUserToken);
                if (passRequest is string errorMessage)
                {
                    ViewData["Error"] = errorMessage;
                    return RedirectToAction("Spelen", new { token = spelToken });
                }
                return Json(new { success = true, data = passRequest });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message +"This is the exception message" });
            }
        }
        


        public async Task<IActionResult> Leave(string spelToken)
        {
            // string spelToken = token;
            if (spelToken == null)
            {
                Console.WriteLine("SpelToken is null: "+ spelToken);
                return RedirectToAction(nameof(Index));
            }

            Spel spel = await _apiService.GetSpelByToken(spelToken);
            if (spel == null) return RedirectToAction(nameof(Index));

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Speler speler1 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler1Token);
            if (speler1 == null) return NotFound();

            var deleteRequest = _apiService.Delete(spelToken);

            if (spel.Speler2Token == null)
            {
                if (!deleteRequest) return BadRequest();

                return RedirectToAction(nameof(Index));
            }

            Speler speler2 = _context.Spelers.First(s => s.Guid == spel.Speler2Token);
            if (speler2 == null) return NotFound();


            if (currentUserId == speler1.Guid)
            {
                speler2.AantalGewonnen++;
                speler1.AantalVerloren++;
            }
            else if (currentUserId == speler2.Guid)
            {
                speler1.AantalGewonnen++;
                speler2.AantalVerloren++;
            }

            await _context.SaveChangesAsync();

            if (!deleteRequest) return BadRequest();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Result(string token, string winner)
        {
            

            if (token == null || winner == null)
            {
                return BadRequest( "winner is null " +winner+ " or " + "token is null " + token);

            }
            
            Console.WriteLine("winner is  " +winner+ " & " + "token is  " + token);

            var spel = await _apiService.GetSpelByToken(token);
            
            Console.WriteLine($"this is the spel: {spel}");


            if (spel.Speler1Token == null || spel.Speler2Token == null)
            {
                Console.WriteLine("Speler1 is null " +spel.Speler1Token + " & " + "Speler2 is null " + spel.Speler2Token);
            }
            // Retrieve the players
            var speler1 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler1Token);
            var speler2 = _context.Spelers.FirstOrDefault(s => s.Guid == spel.Speler2Token);

            Console.WriteLine("Speler1: " + speler1.Naam + " & " + "Spelr2: " + speler2.Naam);
            if (speler1 == null || speler2 == null)
            {
                TempData["Message"] = "Speler1 || speler2 is null";
                return RedirectToAction("Spelen");
            }

            // Update the win count based on the winner
            if (winner.ToLower() == "white")
            {
                speler1.AantalGewonnen++;
                speler2.AantalVerloren++;
            }
            else if (winner.ToLower() == "black")
            {
                speler2.AantalGewonnen++;
                speler1.AantalVerloren++;
            }
            else if (winner.ToLower() == "It's a tie")
            {
                speler1.AantalGelijk++;
                speler2.AantalGelijk++;
            }
            else
            {
                return BadRequest("Invalid winner. Winner must be either 'white' or 'black'.");
            }

            // Save the changes
            await _context.SaveChangesAsync();

            var deleteRequest = _apiService.Delete(token);
            if (!deleteRequest) return BadRequest("Error occurred by deleting the game inside the result");

            return RedirectToAction(nameof(Index), "Spelers");
        }

        // Get/ create page 
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Omschrijving")] Spel spel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        // _logger.LogError(error.ErrorMessage);
                        ModelState.AddModelError("Error", "Modelstate niet valid.");
                    }
                }

                return View(spel);
            }

            try
            {
                var currentUserID = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //! Checking if the player is already associated with a game
                var existingSpel = await _apiService.GetSpelFromSpeler1Token(currentUserID);
                if (existingSpel != null)
                {
                    // Show a message that they need to finish their current game first
                    ModelState.AddModelError("Error", "Maak je spel af!");
                    return View();
                }

                //! in below case, the existing spel is null, because there is no player associated with the spel 

                // Sets the Speler1Token property to the current player
                spel.Speler1Token = currentUserID;

                // Create a new SpelInfoApi object
                var spelInfo = new SpelInfoApi
                {
                    SpelerToken = currentUserID,
                    SpelOmschrijving = spel.Omschrijving
                };

                // Call the CreateSpel method in the ApiService and return the new created game as an object to pass the token to the spelen view
                var newSpel = await _apiService.CreateSpel(spelInfo);

                // return RedirectToAction("Spelen", new { token = newSpel.Token });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                

                // You might also want to return a view with the error message
                ModelState.AddModelError(string.Empty, "An error occurred while creating a new game. Please try again later.");
                ModelState.AddModelError("Error", ex.Message);

                return View(spel);
            }
        }


        //create new game 
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Omschrijving")] Spel spel)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         foreach (var modelState in ViewData.ModelState.Values)
        //         {
        //             foreach (var error in modelState.Errors)
        //             {
        //                 Console.WriteLine(error.ErrorMessage);
        //                 ModelState.AddModelError("Error", "Modelstate niet valid.");
        //             }
        //         }
        //
        //         return View(spel);
        //     }
        //
        //     try
        //     {
        //         var currentUserID = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //
        //         //! Checking if the player is already associated with a game
        //         var existingSpel = await _apiService.GetSpelFromSpelerToken(currentUserID);
        //         if (existingSpel != null)
        //         {
        //             // Show a message that they need to finish their current game first
        //             ModelState.AddModelError("Error", "Je zit al in een spel!");
        //
        //             return View();
        //         }
        //
        //         //! in below case, the existing spel is null, because there is no player associated with the spel 
        //
        //         // Sets the Speler1Token property to the current player
        //         spel.Speler1Token = currentUserID;
        //
        //         // Create a new SpelInfoApi object
        //         var spelInfo = new SpelInfoApi
        //         {
        //             SpelerToken = currentUserID,
        //             SpelOmschrijving = spel.Omschrijving
        //         };
        //
        //         // Call the CreateSpel method in the ApiService and return the new created game as an object to pass the token to the spelen view
        //         var newSpel = await _apiService.CreateSpel(spelInfo);
        //
        //         // return RedirectToAction("Spelen", new { token = newSpel.Token });
        //         return RedirectToAction("Index");
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception
        //         Console.WriteLine(ex.Message);
        //     }
        //
        //     return View(spel);
        // }

        //Todo: scores 
        // [HttpGet]
        // public async Task<ActionResult> Scores()
        // {
        //     ClaimsPrincipal currentUser = this.User;
        //     var currentUserID = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    
        //
        //     return View(_spelerData.GetSpelers());
        // }


        //Todo: results 
        // [HttpGet]
        // public async Task<IActionResult> Result()
        // {
        //     ClaimsPrincipal currentUser = this.User;
        //     var currentUserID = currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    
        //     
        //     if (_spelerData.GetSpeler(currentUserID) == null)
        //     {
        //     
        //         await _spelerData.AddSpeler(new Speler { Guid = currentUserID, Naam=currentUser.Identity.Name});
        //         Debug.WriteLine($"added new player:{currentUserID} ");
        //     }
        //    
        //
        //     Speler speler = _spelerData.GetSpeler(currentUserID);
        //
        //     Kleur winnaar = await _speldata.getWinnaarBySpelertoken(currentUserID);
        //     if (winnaar == await _speldata.GetKleur(currentUserID))
        //     {
        //         speler.AantalGewonnen++;
        //         _spelerData.SaveChanges();
        //       
        //         ViewData.Model = "je hebt gewonnen!";
        //         _speldata.SpelDone(currentUserID);
        //         return View();
        //     }
        //     else
        //     {
        //         speler.AantalVerloren++;
        //         _spelerData.SaveChanges();
        //         _speldata.SpelDone(currentUserID);
        //         ViewData.Model = "je hebt verloren!";
        //         return View();
        //     }
        //     
        // }


        // GET: Spellen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Spel == null)
            {
                return NotFound();
            }

            var spel = await _context.Spel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }


        [Authorize(Roles = "Admin")]
        // GET: Spellen/Delete/5
        public async Task<IActionResult> Delete(string token)
        {
            if (token == null)
            {
                return NotFound();
            }

            var spel = await _apiService.GetSpelByToken(token);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        [Authorize(Roles = "Admin")]
        // ! // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string token)
        {
            if (_context.Spel == null)
            {
                return Problem("Entity set 'ReversiDbContext.Spellen'  is null.");
            }

            var spel = await _apiService.GetSpelByToken(token);
            if (spel != null)
            {
                _apiService.Delete(token);
            }

            // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}