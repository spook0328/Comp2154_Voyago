using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TripPlanner.Models;

public class Itinerary
{
    // PK
    [Key]
    [Column("itinerary_id")]
    public int ItineraryId { get; set; }
    
    // FK From User ↓
    [Required]
    [Column("user_id")]
    public string UserId { get; set; }
    
    // FK From Country ↓
    [Column("country_id")]
    public int? CountryId { get; set; } 
    
    [Required]
    [StringLength(255)]
    [Column("title")]
    public string Title { get; set; } = null!;
    
    [Required]
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationUser User { get; set; } = null!;
    public Country? Country { get; set; }
    public List<ItineraryItem> ItineraryItems { get; set; } = new();

}