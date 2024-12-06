using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace olx_assistant_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TargetTasks",
                table: "TargetTasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "TargetTasks");

            migrationBuilder.AddColumn<string>(
                name: "jobId",
                table: "TargetTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProcessedByTaskId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TargetTasks",
                table: "TargetTasks",
                column: "jobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TargetTasks",
                table: "TargetTasks");

            migrationBuilder.DropColumn(
                name: "jobId",
                table: "TargetTasks");

            migrationBuilder.DropColumn(
                name: "ProcessedByTaskId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "TargetTasks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TargetTasks",
                table: "TargetTasks",
                column: "TaskId");
        }
    }
}
