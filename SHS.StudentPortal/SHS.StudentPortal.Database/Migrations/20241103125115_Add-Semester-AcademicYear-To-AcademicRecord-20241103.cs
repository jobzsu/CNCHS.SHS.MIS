using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHS.StudentPortal.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterAcademicYearToAcademicRecord20241103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AcademicRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "AcademicRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AcademicRecords");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "AcademicRecords");
        }
    }
}
