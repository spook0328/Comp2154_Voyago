using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItineraryItemAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_itinerary_items_ItineraryId",
                table: "itinerary_items");

            migrationBuilder.InsertData(
                table: "itineraries",
                columns: new[] { "itinerary_id", "CountryId", "Description", "EndDate", "StartDate", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2026, 3, 14, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "First Trip", null },
                    { 2, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Second Trip", null }
                });

            migrationBuilder.InsertData(
                table: "itinerary_items",
                columns: new[] { "ItineraryItemId", "EndDateTime", "ItineraryId", "Note", "StartDateTime", "StopOrder" },
                values: new object[,]
                {
                    { 1, null, 1, null, new DateTime(2026, 1, 16, 9, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, null, 1, null, new DateTime(2026, 2, 16, 9, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, null, 2, null, new DateTime(2026, 3, 16, 9, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 4, null, 2, null, new DateTime(2026, 4, 16, 9, 0, 0, 0, DateTimeKind.Utc), 4 }
                });

            migrationBuilder.InsertData(
                table: "locations",
                columns: new[] { "LocationId", "Address", "Description", "ItineraryItemId", "Latitude", "Longitude", "Name", "PlaceId" },
                values: new object[,]
                {
                    { 1, "N/A For Test", null, 1, 45.504537m, -73.556094m, "Notre-Dame Basilica", null },
                    { 2, "Isfahan, Isfahan Province, Iran", null, 2, 32.65745m, 51.677778m, "Naqsh-e Jahan Square", null },
                    { 3, "Yuchi Township, Nantou County, Taiwan", null, 3, 23.866667m, 120.916667m, "Sun Moon Lake", null },
                    { 4, "Tuojiang Town, Fenghuang County, Xiangxi Tujia and Miao Autonomous Prefecture of Hunan Province", null, 3, 27.952822m, 109.600989m, "Fenghuang Ancient City", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_itinerary_items_ItineraryId_StopOrder",
                table: "itinerary_items",
                columns: new[] { "ItineraryId", "StopOrder" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_itinerary_items_ItineraryId_StopOrder",
                table: "itinerary_items");

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "ItineraryItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "LocationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "LocationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "LocationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "LocationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "ItineraryItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "ItineraryItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "ItineraryItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "itineraries",
                keyColumn: "itinerary_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "itineraries",
                keyColumn: "itinerary_id",
                keyValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_itinerary_items_ItineraryId",
                table: "itinerary_items",
                column: "ItineraryId");
        }
    }
}
