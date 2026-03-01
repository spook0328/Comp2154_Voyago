using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.Models;

public class Phrase
{
    // PK
    [Column("phrase_id")]
    public int PhraseId { get; set; } 
    
    // FK From Country
    [Required]
    [Column("country_id")]
    public int CountryId { get; set; }
    public Country Country { get; set; } 
    
    [Required]
    [Column("content")]
    public string Content { get; set; } = null!;
    
    [Required]
    [Column("translation")]
    public string Translation { get; set; } = null!;

}