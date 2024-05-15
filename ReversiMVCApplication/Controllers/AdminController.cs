using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMVCApplication.Data;
using ReversiMVCApplication.Models;
using ReversiMVCApplication.Services;

namespace ReversiMVCApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ReversiDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApiService _apiService;

        public AdminController(ReversiDbContext context, ApiService apiService, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _apiService = apiService;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            // IdentityUser? speler2 = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == s.Speler2Token);
            //
            // return _context.AdminRequest != null
            //     ? View(await _context.AdminRequest.ToListAsync())
            //     : Problem("Entity set 'ReversiDbContext.AdminRequest'  is null.");
            if (_context.AdminRequest == null)
            {
                return Problem("Entity set 'ReversiDbContext.AdminRequest'  is null.");
            }

            var adminRequests = await _context.AdminRequest.ToListAsync();

            // Create a new list to hold the view models
            List<AdminRequestViewModel> adminRequestViewModels = new List<AdminRequestViewModel>();

            foreach (var request in adminRequests)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                // show users that are not already an admin
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    // Create a new view model and add it to the list
                    adminRequestViewModels.Add(new AdminRequestViewModel
                    {
                        Id = request.Id,
                        UserId = request.UserId,
                        UserName = user?.UserName,
                        RequestDate = request.RequestDate,
                        Status = request.Status
                    });
                }
            }

            return View(adminRequestViewModels);
        }
        
        
        public async Task<IActionResult> AllGames()
        {
            List<Spel> spelInfo = await _apiService.GetSpelInfo();

            // Create a new list to hold the view models
            List<SpelViewModel> spelViewModels = new List<SpelViewModel>();

            foreach (var s in spelInfo)
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


        //Users 
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var userRoleViewModels = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userRoleViewModels.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = userRoles,
                    AllRoles = roles.Select(r => r.Name).ToList()
                });
            }

            return View(userRoleViewModels);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(Users));
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdminRequest == null)
            {
                return NotFound();
            }

            var adminRequest = await _context.AdminRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminRequest == null)
            {
                return NotFound();
            }

            return View(adminRequest);
        }

//         // GET: Admin/Create
//         public IActionResult Create()
//         {
//             return View();
//         }
//
//         // POST: Admin/Create
// // To protect from overposting attacks, enable the specific properties you want to bind to.
// // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create([Bind("Id,UserId,RequestDate,Status")] AdminRequest adminRequest)
//         {
//             IdentityUser? userName = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == adminRequest.UserId);
//
//             if (ModelState.IsValid)
//             {
//                 // Look up the user in the database
//                 var user = await _userManager.FindByIdAsync(adminRequest.UserId);
//                 if (user == null)
//                 {
//                     // If the user does not exist, return an error
//                     ModelState.AddModelError("", "User does not exist.");
//                     return View(adminRequest);
//                 }
//
//                 // Set the UserName property
//                 adminRequest.UserId = user.;
//
//                 _context.Add(adminRequest);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//
//             return View(adminRequest);
//         }

        // GET: Admin/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null || _context.AdminRequest == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var adminRequest = await _context.AdminRequest.FindAsync(id);
        //     if (adminRequest == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(adminRequest);
        // }
        //
        // // POST: Admin/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RequestDate,Status")] AdminRequest adminRequest)
        // {
        //     if (id != adminRequest.Id)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(adminRequest);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!AdminRequestExists(adminRequest.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //
        //         return RedirectToAction(nameof(Index));
        //     }
        //
        //     return View(adminRequest);
        // }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdminRequest == null)
            {
                return NotFound();
            }

            var adminRequest = await _context.AdminRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminRequest == null)
            {
                return NotFound();
            }

            return View(adminRequest);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdminRequest == null)
            {
                return Problem("Entity set 'ReversiDbContext.AdminRequest'  is null.");
            }

            var adminRequest = await _context.AdminRequest.FindAsync(id);
            if (adminRequest != null)
            {
                _context.AdminRequest.Remove(adminRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminRequestExists(int id)
        {
            return (_context.AdminRequest?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<IActionResult> HandleAdminRequest([FromBody] AdminRequest adminRequest)
        {
            if (adminRequest == null)
            {
                return BadRequest("Admin request is null.");
            }

            _context.AdminRequest.Add(adminRequest);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptAdminRequest(int id)
        {
            var adminRequest = await _context.AdminRequest.FindAsync(id);
            if (adminRequest == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(adminRequest.UserId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            _context.AdminRequest.Remove(adminRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenyAdminRequest(int id)
        {
            var adminRequest = await _context.AdminRequest.FindAsync(id);
            if (adminRequest == null)
            {
                return NotFound();
            }

            _context.AdminRequest.Remove(adminRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}