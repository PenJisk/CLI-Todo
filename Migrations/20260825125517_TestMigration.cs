using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListConsole.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "SaveRecords");

            migrationBuilder.AddColumn<string>(
                name: "TaskStatus",
                table: "SaveRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "SaveRecords");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "SaveRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
