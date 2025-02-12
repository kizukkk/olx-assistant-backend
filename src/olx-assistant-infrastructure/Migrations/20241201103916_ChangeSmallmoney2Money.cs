using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace olx_assistant_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSmallmoney2Money : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "smallmoney");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "smallmoney",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}
