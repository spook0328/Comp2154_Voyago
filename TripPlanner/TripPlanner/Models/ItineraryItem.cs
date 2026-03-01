using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class ItineraryItem
{
    // PK
    [Column("itinerary_item_id")]
    public int ItineraryItemId { get; set; }
    
    // FK From Itinerary ↓
    [Required]
    [Column("itinerary_id")]
    public int ItineraryId { get; set; }
    
    
    [Required]
    [Column("start_date_time")]
    public DateTime StartDateTime { get; set; }
    
    [Column("end_date_time")]
    public DateTime? EndDateTime { get; set; }
    
    [Required]
    [Column("stop_order")]
    public int StopOrder { get; set; }
    
    [Column("note")]
    public string? Note { get; set; }
    
    public Itinerary Itinerary { get; set; }
    public List<Location> Locations { get; set; } = new();

}