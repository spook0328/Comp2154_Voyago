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
            
        // TODO: translate
        // === 表名映射部分 ===
        modelBuilder.Entity<Country>().ToTable("countries");
        modelBuilder.Entity<Itinerary>().ToTable("itineraries");
        modelBuilder.Entity<ItineraryItem>().ToTable("itinerary_items");
        modelBuilder.Entity<Location>().ToTable("locations");
        modelBuilder.Entity<Phrase>().ToTable("phrases");
        
        // TODO: translate
        // === 关系映射部分 ===
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

        // === ItineraryAttraction join table - composite PK ===
        modelBuilder.Entity<ItineraryAttraction>()
            .HasKey(ia => new { ia.ItineraryId, ia.AttractionId });

        modelBuilder.Entity<ItineraryAttraction>()
            .HasOne(ia => ia.Itinerary)
            .WithMany(i => i.ItineraryAttractions)
            .HasForeignKey(ia => ia.ItineraryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItineraryAttraction>()
            .HasOne(ia => ia.Attraction)
            .WithMany(a => a.ItineraryAttractions)
            .HasForeignKey(ia => ia.AttractionId)
            .OnDelete(DeleteBehavior.Cascade);

        // TODO: add attraction field mappings/constaints here once fields are defined in the Attraction model
    }
}