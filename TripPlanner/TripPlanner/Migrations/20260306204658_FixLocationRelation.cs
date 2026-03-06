using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Migrations
{
    /// <inheritdoc />
    public partial class FixLocationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItineraryItemId",
                table: "locations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItineraryItemId",
                table: "locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
