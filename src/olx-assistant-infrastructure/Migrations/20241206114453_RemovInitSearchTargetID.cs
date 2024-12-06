using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace olx_assistant_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovInitSearchTargetID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Targets_SearchTargetId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SearchTargetId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SearchTargetId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SearchTargetId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SearchTargetId",
                table: "Products",
                column: "SearchTargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Targets_SearchTargetId",
                table: "Products",
                column: "SearchTargetId",
                principalTable: "Targets",
                principalColumn: "Id");
        }
    }
}
