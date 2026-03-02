using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models;

public class Itinerary
{
    public int Id { get; set; }

    // the user who owns this itinerary
    public string? UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    // navigation property back to the user
    public ApplicationUser? User { get; set; }
}