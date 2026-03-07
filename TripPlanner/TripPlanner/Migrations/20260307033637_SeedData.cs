using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TripPlanner.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locations_itinerary_items_ItineraryItemId",
                table: "locations");

            migrationBuilder.DropIndex(
                name: "IX_locations_ItineraryItemId",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "ItineraryItemId",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "LastPhoneNumberChangeDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberChangeCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureMimeType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "locations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItineraryItemId",
                table: "itinerary_items",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "itinerary_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "itinerary_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "itineraries",
                columns: new[] { "itinerary_id", "CountryId", "Description", "EndDate", "StartDate", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2026, 3, 14, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "First Trip", null },
                    { 2, null, null, new DateTime(2026, 6, 14, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Second Trip", null }
                });

            migrationBuilder.InsertData(
                table: "locations",
                columns: new[] { "Id", "Address", "Description", "Latitude", "Longitude", "Name", "PlaceId" },
                values: new object[,]
                {
                    { 1, "N/A For Test", null, 45.504537m, -73.556094m, "Notre-Dame Basilica", null },
                    { 2, "Isfahan, Isfahan Province, Iran", null, 32.65745m, 51.677778m, "Naqsh-e Jahan Square", null },
                    { 3, "Yuchi Township, Nantou County, Taiwan", null, 23.866667m, 120.916667m, "Sun Moon Lake", null },
                    { 4, "Tuojiang Town, Fenghuang County, Xiangxi Tujia and Miao Autonomous Prefecture of Hunan Province", null, 27.952822m, 109.600989m, "Fenghuang Ancient City", null }
                });

            migrationBuilder.InsertData(
                table: "itinerary_items",
                columns: new[] { "Id", "EndDateTime", "ItineraryId", "LocationId", "Note", "StartDateTime", "StopOrder" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 15, 9, 0, 0, 0, DateTimeKind.Utc), 1, 1, null, new DateTime(2026, 1, 16, 9, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, new DateTime(2026, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), 1, 2, null, new DateTime(2026, 2, 16, 9, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, new DateTime(2026, 4, 15, 9, 0, 0, 0, DateTimeKind.Utc), 2, 3, null, new DateTime(2026, 3, 16, 9, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 4, new DateTime(2026, 5, 15, 9, 0, 0, 0, DateTimeKind.Utc), 2, 4, null, new DateTime(2026, 4, 16, 9, 0, 0, 0, DateTimeKind.Utc), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_itinerary_items_LocationId",
                table: "itinerary_items",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_itinerary_items_locations_LocationId",
                table: "itinerary_items",
                column: "LocationId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itinerary_items_locations_LocationId",
                table: "itinerary_items");

            migrationBuilder.DropIndex(
                name: "IX_itinerary_items_LocationId",
                table: "itinerary_items");

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "itinerary_items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "itineraries",
                keyColumn: "itinerary_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "itineraries",
                keyColumn: "itinerary_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "locations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "itinerary_items");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "locations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "itinerary_items",
                newName: "ItineraryItemId");

            migrationBuilder.AddColumn<int>(
                name: "ItineraryItemId",
                table: "locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "itinerary_items",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPhoneNumberChangeDate",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumberChangeCount",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureMimeType",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_locations_ItineraryItemId",
                table: "locations",
                column: "ItineraryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_locations_itinerary_items_ItineraryItemId",
                table: "locations",
                column: "ItineraryItemId",
                principalTable: "itinerary_items",
                principalColumn: "ItineraryItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
