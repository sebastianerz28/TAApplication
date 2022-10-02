using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Data.Migrations
{
    public partial class AddApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreePursuing = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GPA = table.Column<double>(type: "float", nullable: false),
                    DesiredHours = table.Column<int>(type: "int", nullable: false),
                    AvailableBeforeSemester = table.Column<bool>(type: "bit", nullable: false),
                    SemestersCompleted = table.Column<int>(type: "int", nullable: false),
                    PersonalStatement = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: true),
                    TransferSchool = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_AspNetUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantId",
                table: "Applications",
                column: "ApplicantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
