using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class ItineraryItem
{
    // PK
    public int Id { get; set; }
    
    // FK
    [Required]
    public int ItineraryId { get; set; }
    [Required]
    public int LocationId { get; set; }
    
    [Required]
    public DateTime StartDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    
    [Required]
    public int StopOrder { get; set; }
    
    public string? Note { get; set; }
    
    // navigation property back to the user
    public Itinerary Itinerary { get; set; } = null!;
    public Location Location { get; set; } = null!;

}