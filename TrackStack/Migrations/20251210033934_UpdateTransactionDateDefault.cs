using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackStack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionDateDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserEmail",
                table: "Expenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserEmail",
                table: "Expenses",
                column: "UserEmail");
        }
    }
}
