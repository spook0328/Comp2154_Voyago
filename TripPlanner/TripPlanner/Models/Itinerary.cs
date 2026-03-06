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
    public ICollection<ItineraryItem> ItineraryItems { get; set; } = new List<ItineraryItem>();
}