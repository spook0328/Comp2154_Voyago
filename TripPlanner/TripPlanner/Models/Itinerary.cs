using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class Itinerary
{
    [Column("itinerary_id")]
    public int Id { get; set; }

    // the user who owns this itinerary
    public string? UserId { get; set; }
    
    public int? CountryId { get; set; } 

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
    public Country? Country { get; set; }
    public List<ItineraryItem> ItineraryItems { get; set; } = new();

    // navigation property to the attractions included in this itinerary 
    // TODO: We may want to change this to a many-to-many relationship if we want to allow the same attraction to be included in multiple itineraries without duplication
    public List<ItineraryAttraction> ItineraryAttractions { get; set; } = new();
}