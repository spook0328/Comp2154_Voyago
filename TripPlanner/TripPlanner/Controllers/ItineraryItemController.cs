using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Data;
using TripPlanner.Dtos.ItineraryItem;
using TripPlanner.Models;

namespace TripPlanner.Controllers;

[Authorize]
[ApiController]
[Authorize(Roles = "Admin")]
[Route("itineraries/{itineraryId:int}/items")]
public class ItineraryItemController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    // Ownership Verification Core
    public ItineraryItemController(
        // Get Itineraries, ItineraryItems
        ApplicationDbContext context,
        
        // Get User ID from sign in status
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    // Get Current User Id
    private string GetCurrentUserId()
    {
        return _userManager.GetUserId(User);
    }
    
    // Itinerary Ownership Check
    private async Task<Itinerary?> GetOwnedItineraryAsync(int itineraryId)
    {
        // Fetch the itinerary ID
        var itinerary = await _context.Itineraries
            .FirstOrDefaultAsync(i => i.Id == itineraryId);

        if (itinerary == null)
            return null;

        // Full permission for admin
        if (User.IsInRole("Admin"))
            return itinerary;

        // Get user's own itineraries
        var userId = GetCurrentUserId();
        if (itinerary.UserId == userId)
            return itinerary;

        return null;
    }
            
    
    // Create Item (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateItem(int itineraryId,
        CreateItineraryItemDto dto)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null)
            return NotFound();

        var item = new ItineraryItem
        {
            ItineraryId = itineraryId,
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            StopOrder = dto.StopOrder,
            Note = dto.Note
        };

        _context.ItineraryItems.Add(item);
        await _context.SaveChangesAsync();

        var resultDto = new ItineraryItemDto
        {
            Id = item.Id,
            StartDateTime = item.StartDateTime,
            EndDateTime = item.EndDateTime,
            StopOrder = item.StopOrder,
            Note = item.Note
        };

        return Ok(resultDto);
    }
    
    // Read Items (GET)
    [HttpGet]
    public async Task<IActionResult> GetItems(int itineraryId)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null)
            return NotFound();

        var items = await _context.ItineraryItems
            .Where(i => i.ItineraryId == itineraryId)
            .OrderBy(i => i.StopOrder)
            .ToListAsync();

        var itemsDto = items.Select(i => new ItineraryItemDto
        {
            Id = i.Id,
            StartDateTime = i.StartDateTime,
            EndDateTime = i.EndDateTime,
            StopOrder = i.StopOrder,
            Note = i.Note
        }).ToList();

        return Ok(itemsDto);
    }
    
    // Update Item (PUT)
    [HttpPut("{itemId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateItem(
        int itineraryId,
        int itemId,
        UpdateItineraryItemDto dto)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null)
            return NotFound();

        var item = await _context.ItineraryItems
            .FirstOrDefaultAsync(i =>
                i.Id == itemId &&
                i.ItineraryId == itineraryId);

        if (item == null)
            return NotFound();

        item.StartDateTime = dto.StartDateTime;
        item.EndDateTime = dto.EndDateTime;
        item.StopOrder = dto.StopOrder;
        item.Note = dto.Note;

        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    // Delete Item (DELETE)
    [HttpDelete("{itemId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteItem(
        int itineraryId,
        int itemId)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null)
            return NotFound();

        var item = await _context.ItineraryItems
            .FirstOrDefaultAsync(i =>
                i.Id == itemId &&
                i.ItineraryId == itineraryId);

        if (item == null)
            return NotFound();

        _context.ItineraryItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
}


