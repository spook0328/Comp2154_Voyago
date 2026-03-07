using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Dtos.ItineraryItem;

public class CreateItineraryItemDto
{
    [Required]
    public int LocationId { get; set; }
    
    [Required]
    public DateTime StartDateTime { get; set; }
    
    
    public DateTime EndDateTime { get; set; }
    
    [Required]
    public int StopOrder { get; set; }
    
    public string? Note { get; set; }
}