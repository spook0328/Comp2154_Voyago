using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Models;

namespace TripPlanner.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Itinerary> Itineraries => Set<Itinerary>();
    public DbSet<ItineraryItem> ItineraryItems => Set<ItineraryItem>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Phrase> Phrases => Set<Phrase>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        // === Table name mapping part ===
        modelBuilder.Entity<Country>().ToTable("countries");
        modelBuilder.Entity<Itinerary>().ToTable("itineraries");
        modelBuilder.Entity<ItineraryItem>().ToTable("itinerary_items");
        modelBuilder.Entity<Location>().ToTable("locations");
        modelBuilder.Entity<Phrase>().ToTable("phrases");
        
        // === Index Configuration Section ===
        modelBuilder.Entity<ItineraryItem>()
            .HasIndex(i => new { i.ItineraryId, i.StopOrder })
            .IsUnique();
        
        // === Relationship mapping part ===
        modelBuilder.Entity<Itinerary>()
            .HasOne(i => i.User)
            .WithMany(u => u.Itineraries)
            .HasForeignKey(I => I.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Itinerary>()
            .HasOne(i => i.Country)
            .WithMany(c => c.Itineraries)
            .HasForeignKey(I => I.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ItineraryItem>()
            .HasOne(t => t.Itinerary)
            .WithMany(i => i.ItineraryItems)
            .HasForeignKey(I => I.ItineraryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Location>()
            .HasOne(i => i.ItineraryItem)
            .WithMany(l => l.Locations)
            .HasForeignKey(I => I.ItineraryItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Phrase>()
            .HasOne(p => p.Country)
            .WithMany(c => c.Phrases)
            .HasForeignKey(p => p.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // === Add Seed Data Part ===
        modelBuilder.Entity<Itinerary>()
            .HasData(
                new Itinerary {
                    Id = 1,
                    Title = "First Trip",
                    StartDate = new DateTime(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 3, 14, 9, 0, 0, DateTimeKind.Utc),
                },
                new Itinerary {
                    Id = 2,
                    Title = "Second Trip",
                    StartDate = new DateTime(2026, 3, 15, 9, 0, 0, DateTimeKind.Utc)
                }
                );
        
        modelBuilder.Entity<ItineraryItem>()
            .HasData(
                new ItineraryItem { 
                    ItineraryItemId = 1,
                    ItineraryId = 1,
                    StartDateTime = new DateTime(2026, 1, 16, 9, 0, 0, DateTimeKind.Utc),
                    StopOrder = 1
                    },
                new ItineraryItem { 
                    ItineraryItemId = 2,
                    ItineraryId = 1,
                    StartDateTime = new DateTime(2026, 2, 16, 9, 0, 0, DateTimeKind.Utc),
                    StopOrder = 2
                },
                new ItineraryItem { 
                    ItineraryItemId = 3,
                    ItineraryId = 2,
                    StartDateTime = new DateTime(2026, 3, 16, 9, 0, 0, DateTimeKind.Utc),
                    StopOrder = 3
                },
                new ItineraryItem { 
                    ItineraryItemId = 4,
                    ItineraryId = 2,
                    StartDateTime = new DateTime(2026, 4, 16, 9, 0, 0, DateTimeKind.Utc),
                    StopOrder = 4
                }
                );
        modelBuilder.Entity<Location>()
            .HasData(
                new Location { 
                    LocationId = 1, 
                    ItineraryItemId = 1,
                    Name = "Notre-Dame Basilica", 
                    Address = "N/A For Test",
                    Latitude = 45.504537m, 
                    Longitude = -73.556094m
                    
                }, 
                new Location { 
                    LocationId = 2, 
                    ItineraryItemId = 2,
                    Name = "Naqsh-e Jahan Square",
                    Address = "Isfahan, Isfahan Province, Iran",
                    Latitude = 32.65745m, 
                    Longitude = 51.677778m
                    
                },
                new Location { 
                    LocationId = 3, 
                    ItineraryItemId = 3,
                    Name = "Sun Moon Lake", 
                    Address = "Yuchi Township, Nantou County, Taiwan",
                    Latitude = 23.866667m, 
                    Longitude = 120.916667m
                    
                },
                new Location { 
                    LocationId = 4, 
                    ItineraryItemId = 3,
                    Name = "Fenghuang Ancient City", 
                    Address = "Tuojiang Town, Fenghuang County, Xiangxi Tujia and Miao Autonomous Prefecture of Hunan Province",
                    Latitude = 27.952822m, 
                    Longitude = 109.600989m}
            );
    }
}