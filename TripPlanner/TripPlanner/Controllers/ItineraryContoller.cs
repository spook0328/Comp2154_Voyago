using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TripPlanner.Models;
using TripPlanner.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// This controller is responsible for handling requests related to the itinerary of a trip. It will manage the creation, retrieval, updating, and deletion of itinerary items, as well as any associated logic for displaying the itinerary to the user.
/// </summary>
namespace TripPlanner.Controllers
{
    [Authorize]
    public class ItineraryController : Controller
    {
        // Dependency injection of the database context to interact with the database.
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the database context.
        public ItineraryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get: Itinerary/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                // TODO: Replace "Itineraries" with the actual name of the DbSet in your ApplicationDbContext that represents the itineraries.
                // Admin sees all itineraries
                return View(await _context.Itineraries.ToListAsync());
            }

            // Normal users only see their own itineraries
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return View(await _context.Itineraries
                // TODO: Replace "UserId" with the actual property name in model
                .Where(i => i.UserId == userId)
                .ToListAsync());
        }

        // Get: Itinerary/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the id parameter is null, which means the itinerary was not found.
            if (id == null)
            {
                return NotFound();
            }

            var itinerary = await _context.Itineraries
                // TODO: Include items if needed
                // .Include(i => i.Items) // Uncomment if you have an Items navigation property
                .FirstOrDefaultAsync(m => m.Id == id);

            // Return a 404 Not Found response if itinerary does not exit in the database.
            if (itinerary == null) return NotFound();

            // Ownership protection (non-admin users)
            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (itinerary.UserId != userId)
                    return Forbid();
            }

            return View(itinerary);
        }

        // Get: Itinerary/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Post: Itinerary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/* TODO: Itinerary itinerary */)
        {
            if (ModelState.IsValid)
            {
                // TODO:
                // 1. Set itinerary.UserId = current user id
                // 2. Add itinerary to the database context
                // 3. Save changes to the database

                //itinerary.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //_context.Add(itinerary);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // Get: Itinerary/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the id parameter is null, which means the itinerary was not found.
            if (id == null) return NotFound();

            // TODO: Replace "Itineraries" with the actual name of the DbSet
            var itinerary = await _context.Itineraries.FindAsync(id);

            // Return a 404 Not Found response if itinerary does not exit in the database.
            if (itinerary == null) return NotFound();

            // Ownership protection (non-admin users)
            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (itinerary.UserId != userId) return Forbid();
            }

            return View(itinerary);
        }

        // Post: Itinerary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, /* TODO: Itinerary itinerary */)
        {
            if (id == 0) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Update the itinerary
                    // _context.Update(itinerary);
                    // await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // Get: Itinerary/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // TODO: Replace "Itineraries" with the actual name of the DbSet in your ApplicationDbContext that represents the itineraries.
            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(m => m.Id == id);

            if (itinerary == null) return NotFound();

            // Ownership protection (non-admin users)
            if (!User.IsInRole("Admin"))
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (itinerary.UserId != userId) return Forbid();
            }

            return View(itinerary);
        }

        // Post: Itinerary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // TODO: Replace "Itineraries" with the actual name of the DbSet in your ApplicationDbContext that represents the itineraries.
            var itinerary = await _context.Itineraries.FindAsync(id);

            if (itinerary != null)
            {
                _context.Itineraries.Remove(itinerary);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}