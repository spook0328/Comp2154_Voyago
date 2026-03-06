using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class Location
{
    // PK
    public int LocationId { get; set; }
    
    // FK From ItineraryItem
    [Required]
    public int ItineraryItemId { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(9,6)")]
    public decimal Latitude { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(9,6)")]
    public decimal Longitude { get; set; }
    
    public string? Description { get; set; }
    
    public string? PlaceId { get; set; }
    
    public ItineraryItem ItineraryItem { get; set; } = null!;
   
}