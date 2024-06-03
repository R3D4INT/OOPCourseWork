using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFuethersDeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuethersDeals_Coins_CoinId",
                table: "FuethersDeals");

            migrationBuilder.DropIndex(
                name: "IX_FuethersDeals_CoinId",
                table: "FuethersDeals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FuethersDeals_CoinId",
                table: "FuethersDeals",
                column: "CoinId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuethersDeals_Coins_CoinId",
                table: "FuethersDeals",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
