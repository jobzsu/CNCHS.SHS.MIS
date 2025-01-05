using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHS.StudentPortal.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveacademicRecordtoSubjectdependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicRecords_Subjects_SubjectId",
                table: "AcademicRecords");

            migrationBuilder.DropIndex(
                name: "IX_AcademicRecords_SubjectId",
                table: "AcademicRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_SubjectId",
                table: "AcademicRecords",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicRecords_Subjects_SubjectId",
                table: "AcademicRecords",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }
    }
}
