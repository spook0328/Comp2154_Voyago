using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Migrations
{
    /// <inheritdoc />
    public partial class FixModels : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "itinerary_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_itinerary_items_LocationId",
                table: "itinerary_items",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_itinerary_items_locations_LocationId",
                table: "itinerary_items",
                column: "LocationId",
                principalTable: "locations",
                principalColumn: "LocationId",
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

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "itinerary_items");

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
