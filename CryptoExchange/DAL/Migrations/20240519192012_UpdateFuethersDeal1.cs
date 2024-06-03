using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFuethersDeal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuethersDeals_Users_UserId",
                table: "FuethersDeals");

            migrationBuilder.DropIndex(
                name: "IX_FuethersDeals_UserId",
                table: "FuethersDeals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FuethersDeals_UserId",
                table: "FuethersDeals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuethersDeals_Users_UserId",
                table: "FuethersDeals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
