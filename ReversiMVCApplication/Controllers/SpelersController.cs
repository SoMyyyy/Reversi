using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMVCApplication.Data;
using ReversiMVCApplication.Models;

namespace ReversiMVCApplication.Controllers
{
    [Authorize]
    public class SpelersController : Controller
    {
        private readonly ReversiDbContext _context;

        public SpelersController(ReversiDbContext context)
        {
            _context = context;
        }

        // GET: Spelers
        public async Task<IActionResult> Index()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User?.Identity.Name;
            
            

            var bestaandeSpeler = await _context.Spelers.FindAsync(userId);
            if (bestaandeSpeler == null)
            {
                var nieuweSpeler = new Speler
                {
                    Guid = userId,
                    Naam = userName
                };

                _context.Spelers.Add(nieuweSpeler);
                await _context.SaveChangesAsync();
            }

            return View(await _context.Spelers.ToListAsync());
        }
        
        

        // GET: Spelers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (id == null || _context.Spelers == null || speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }


        [Authorize(Roles = "Admin")]
        // GET: Spelers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var speler = await _context.Spelers.FindAsync(id);

            if (id == null || _context.Spelers == null || speler == null)
            {
                return NotFound();
            }

            return Redirect("/Identity/Account/Manage");
        }

        [Authorize(Roles = "Admin")]
        // GET: Spelers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (id == null || _context.Spelers == null || speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        [Authorize(Roles = "Admin")]
        // POST: Spelers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Spelers == null)
            {
                return Problem("Entity set 'ReversiDbContext.Spelers'  is null.");
            }

            var speler = await _context.Spelers.FindAsync(id);
            if (speler != null)
            {
                _context.Spelers.Remove(speler);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestAdminRole()
        {
            var currentUserID = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserID == null)
            {
                return BadRequest("Current user ID is null.");
            }

            var adminRequest = new AdminRequest
            {
                UserId = currentUserID,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            _context.AdminRequest.Add(adminRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}