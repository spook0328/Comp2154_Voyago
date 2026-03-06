using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Dtos.Location;

public class UpdateLocationDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public decimal Latitude { get; set; }

    [Required]
    public decimal Longitude { get; set; }

    public string? Description { get; set; }

    public string? PlaceId { get; set; }
}