using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace olx_assistant_infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keyword_Targets_TargetId",
                table: "Keyword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Keyword",
                table: "Keyword");

            migrationBuilder.RenameTable(
                name: "Keyword",
                newName: "Keywords");

            migrationBuilder.RenameIndex(
                name: "IX_Keyword_TargetId",
                table: "Keywords",
                newName: "IX_Keywords_TargetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Keywords",
                table: "Keywords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Keywords_Targets_TargetId",
                table: "Keywords",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keywords_Targets_TargetId",
                table: "Keywords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Keywords",
                table: "Keywords");

            migrationBuilder.RenameTable(
                name: "Keywords",
                newName: "Keyword");

            migrationBuilder.RenameIndex(
                name: "IX_Keywords_TargetId",
                table: "Keyword",
                newName: "IX_Keyword_TargetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Keyword",
                table: "Keyword",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Keyword_Targets_TargetId",
                table: "Keyword",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id");
        }
    }
}
