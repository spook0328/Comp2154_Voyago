using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class Phrase
{
    // PK
    public int PhraseId { get; set; } 
    
    // FK From Country
    [Required]
    public int CountryId { get; set; }
    public Country Country { get; set; } 
    
    [Required]
    public string Content { get; set; } = null!;
    
    [Required]
    public string Translation { get; set; } = null!;

}