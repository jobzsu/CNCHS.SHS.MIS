using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHS.StudentPortal.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherSubjectNameforexternal20250106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherSubjectName",
                table: "AcademicRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.RestartSequence(
                name: "SubjectIdSequence",
                startValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherSubjectName",
                table: "AcademicRecords");

            migrationBuilder.RestartSequence(
                name: "SubjectIdSequence",
                startValue: 1L);
        }
    }
}
