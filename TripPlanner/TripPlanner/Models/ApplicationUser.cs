using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TripPlanner.Models;

namespace TripPlanner.Models;

public class ApplicationUser: IdentityUser
{
    public ICollection<Itinerary> Itineraries { get; set; }
}