using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Models;
using TripPlanner.Data;
using TripPlanner.Dtos.Location;

namespace TripPlanner.Controllers;

[ApiController]
[Route("locations")]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LocationController(ApplicationDbContext context)
    {
        _context = context;
    }
        
        
    // Get /locations
    [HttpGet]
    public async Task<IActionResult> GetAll()
    { 
        var locations = await _context.Locations.ToListAsync();
        var dto = locations.Select(l => new LocationDto
        {
            LocationId = l.Id,
            Name = l.Name,
            Address = l.Address,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Description = l.Description,
            PlaceId = l.PlaceId
        });
        return Ok(dto);
    }
        
        
    // Get  /locations/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null) return NotFound();

        var dto = new LocationDto
        {
            LocationId = location.Id,
            Name = location.Name,
            Address = location.Address,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Description = location.Description,
            PlaceId = location.PlaceId
        };
        return Ok(dto);
    }
        
        
    // POST /locations
    [HttpPost]
    public async Task<IActionResult> Create(CreateLocationDto dto)
    {
        var location = new Location
        {
            Name = dto.Name,
            Address = dto.Address,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Description = dto.Description,
            PlaceId = dto.PlaceId
        };
            
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
            
        return Ok(new LocationDto
        {
            LocationId = location.Id,
            Name = location.Name,
            Address = location.Address,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Description = location.Description,
            PlaceId = location.PlaceId
        });
    }
        
    // PUT /locations/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateLocationDto dto)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null) return NotFound();

        location.Name = dto.Name;
        location.Address = dto.Address;
        location.Latitude = dto.Latitude;
        location.Longitude = dto.Longitude;
        location.Description = dto.Description;
        location.PlaceId = dto.PlaceId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

        
        
    // DELETE /locations/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null) return NotFound();

        // 刪除前確認沒有 ItineraryItem 使用此 Location
        var used = await _context.ItineraryItems.AnyAsync(i => i.LocationId == id);
        if (used) return BadRequest("Location is used by an itinerary item.");

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return NoContent();
    }
        
        
        
        
        
        
        
        
        
        
        
        
    
}