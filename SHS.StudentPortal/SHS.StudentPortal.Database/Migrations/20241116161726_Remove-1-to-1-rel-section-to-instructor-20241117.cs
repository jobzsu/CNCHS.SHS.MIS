using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHS.StudentPortal.Database.Migrations
{
    /// <inheritdoc />
    public partial class Remove1to1relsectiontoinstructor20241117 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_InstructorInfos_AdviserId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_AdviserId",
                table: "Sections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sections_AdviserId",
                table: "Sections",
                column: "AdviserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_InstructorInfos_AdviserId",
                table: "Sections",
                column: "AdviserId",
                principalTable: "InstructorInfos",
                principalColumn: "Id");
        }
    }
}
