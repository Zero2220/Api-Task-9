using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileNameAddedIntoStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Students");
        }
    }
}
