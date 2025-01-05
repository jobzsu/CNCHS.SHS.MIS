using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHS.StudentPortal.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addedencodingandverifyingdetails20250106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EncodedById",
                table: "AcademicRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EncodedDate",
                table: "AcademicRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VerifiedById",
                table: "AcademicRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedDate",
                table: "AcademicRecords",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncodedById",
                table: "AcademicRecords");

            migrationBuilder.DropColumn(
                name: "EncodedDate",
                table: "AcademicRecords");

            migrationBuilder.DropColumn(
                name: "VerifiedById",
                table: "AcademicRecords");

            migrationBuilder.DropColumn(
                name: "VerifiedDate",
                table: "AcademicRecords");
        }
    }
}
