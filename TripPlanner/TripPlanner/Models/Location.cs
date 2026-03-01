using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class Location
{
    // PK
    [Column("location_id")]
    public int LocationId { get; set; }
    
    // FK From ItineraryItem
    [Required]
    [Column("itinerary_item_id")]
    public int ItineraryItemId { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Required]
    [Column("address")]
    public string Address { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(9,6)")]
    public decimal Latitude { get; set; }
    [Required]
    [Column(TypeName = "decimal(9,6)")]
    public decimal Longitude { get; set; }
    public string? Description { get; set; }
    
    [Column("place_id")]
    public string? PlaceId { get; set; }
    
    public ItineraryItem ItineraryItem { get; set; } = null!;
   
}