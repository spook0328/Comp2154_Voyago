using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Migrations
{
    /// <inheritdoc />
    public partial class FixModels2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "locations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "itinerary_items",
                newName: "ItineraryItemId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "itinerary_items",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
