using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class ItineraryItem
{
    // PK
    public int ItineraryItemId { get; set; }
    
    // FK From Itinerary ↓
    [Required]
    public int ItineraryId { get; set; }
    
    
    [Required]
    public DateTime StartDateTime { get; set; }
    
    public DateTime? EndDateTime { get; set; }
    
    [Required]
    public int StopOrder { get; set; }
    
    public string? Note { get; set; }
    
    public Itinerary Itinerary { get; set; }
    public List<Location> Locations { get; set; } = new();

}