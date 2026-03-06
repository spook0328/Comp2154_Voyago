namespace TripPlanner.Dtos.Location;

public class LocationDto
{
    public int LocationId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Address { get; set; } = null!;
    
    public decimal Latitude { get; set; }
    
    public decimal Longitude { get; set; }
    
    public string? Description { get; set; }
    
    public string? PlaceId { get; set; }
}