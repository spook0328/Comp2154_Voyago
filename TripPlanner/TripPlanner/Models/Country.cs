using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class Country
{
    // PK
    public int CountryId { get; set; } 
    
    [Required]
    [MaxLength(50)]
    public string CountryName { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string CountryLanguage { get; set; } = null!;
        
    public List<Itinerary> Itineraries { get; set; } = new();
    public List<Phrase> Phrases { get; set; } = new();
}