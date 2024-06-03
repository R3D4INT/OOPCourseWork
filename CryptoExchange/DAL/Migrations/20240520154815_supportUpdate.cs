using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class supportUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tickets_TicketInProgressId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tickets_TicketInProgressId",
                table: "Users",
                column: "TicketInProgressId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tickets_TicketInProgressId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tickets_TicketInProgressId",
                table: "Users",
                column: "TicketInProgressId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
