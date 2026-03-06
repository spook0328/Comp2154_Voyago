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
    private string GetCurrentUserId() => _userManager.GetUserId(User);
    
    // Itinerary Ownership Check
    private async Task<Itinerary?> GetOwnedItineraryAsync(int itineraryId)
    {
        // Fetch the itinerary ID
        var itinerary = await _context.Itineraries
            .FirstOrDefaultAsync(i => i.Id == itineraryId);

        if (itinerary == null) return null;

        // Full permission for admin
        if (User.IsInRole("Admin")) return itinerary;

        // Get user's own itineraries
        var userId = GetCurrentUserId();
        if (itinerary.UserId == userId)
            return itinerary;

        return null;
    }
    
    
    
    // Read Items (GET)
    [HttpGet]
    public async Task<IActionResult> GetItems(int itineraryId)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null) return NotFound();

        var items = await _context.ItineraryItems
            .Include(i => i.Location)
            .Where(i => i.ItineraryId == itineraryId)
            .OrderBy(i => i.StopOrder)
            .ToListAsync();

        var itemsDto = items.Select(i => new ItineraryItemDto
        {
            Id = i.Id,
            LocationId = i.LocationId,
            LocationName = i.Location.Name,
            StartDateTime = i.StartDateTime,
            EndDateTime = i.EndDateTime,
            StopOrder = i.StopOrder,
            Note = i.Note
        });

        return Ok(itemsDto);
    }
    
    
    
    // Create Item (POST) /itineraries/{itineraryId}/items
    [HttpPost]
    public async Task<IActionResult> CreateItem(int itineraryId,
        CreateItineraryItemDto dto)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null) return NotFound();
        
        var locationExists = await _context.Locations.AnyAsync(l => l.Id == dto.LocationId);
        if (!locationExists) return BadRequest("Location not found.");

        var item = new ItineraryItem
        {
            ItineraryId = itineraryId,
            LocationId = dto.LocationId,
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
            LocationId = item.LocationId,
            LocationName = (await _context.Locations.FindAsync(item.LocationId))!.Name,
            StartDateTime = item.StartDateTime,
            EndDateTime = item.EndDateTime,
            StopOrder = item.StopOrder,
            Note = item.Note
        };

        return Ok(resultDto);
    }
    
    
    
    // Update Item (PUT)
    [HttpPut("{itemId:int}")]
    public async Task<IActionResult> UpdateItem(
        int itineraryId,
        int itemId,
        UpdateItineraryItemDto dto)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null) return NotFound();

        var item = await _context.ItineraryItems
            .FirstOrDefaultAsync(i =>
                i.Id == itemId &&
                i.ItineraryId == itineraryId);

        if (item == null) return NotFound();

        var locationExists = await _context.Locations.AnyAsync(l => l.Id == dto.LocationId);
        if (!locationExists) return BadRequest("Location not found.");

        item.LocationId = dto.LocationId;
        item.StartDateTime = dto.StartDateTime;
        item.EndDateTime = dto.EndDateTime;
        item.StopOrder = dto.StopOrder;
        item.Note = dto.Note;

        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    
    
    // Delete Item (DELETE) /itineraries/{itineraryId}/items/{itemId}
    [HttpDelete("{itemId:int}")]
    public async Task<IActionResult> DeleteItem(
        int itineraryId,
        int itemId)
    {
        var itinerary = await GetOwnedItineraryAsync(itineraryId);
        if (itinerary == null) return NotFound();

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


