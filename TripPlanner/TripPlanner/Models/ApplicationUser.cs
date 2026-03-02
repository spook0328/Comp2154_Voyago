using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TripPlanner.Models;

namespace TripPlanner.Models;

public class ApplicationUser: IdentityUser
{
    // Need to discuss what will be stored
    public byte[]? ProfilePicture { get; set; } 
    public string? ProfilePictureMimeType { get; set; } 
    public DateTime? LastPhoneNumberChangeDate { get; set; } 
    public int PhoneNumberChangeCount { get; set; } = 0;
    public ICollection<Itinerary> Itineraries { get; set; }
}