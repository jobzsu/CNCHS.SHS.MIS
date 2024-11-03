using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHS.StudentPortal.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterAndAcademicYearToStudentSchedulesTable20241027 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "StudentSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "StudentSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "StudentSchedules");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "StudentSchedules");
        }
    }
}
