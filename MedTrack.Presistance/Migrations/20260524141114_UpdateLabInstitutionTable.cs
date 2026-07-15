using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedTrack.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabInstitutionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "Visits",
                type: "decimal(4,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AlterColumn<int>(
                name: "HeartRate",
                table: "Visits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "BloodPressure",
                table: "Visits",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "LabTests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LabInstitutionId",
                table: "LabTests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VisitId",
                table: "LabTests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LabInstitution",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionType = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstablishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabInstitution", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabTests_LabInstitutionId",
                table: "LabTests",
                column: "LabInstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTests_VisitId",
                table: "LabTests",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_LabInstitution_LabInstitutionId",
                table: "LabTests",
                column: "LabInstitutionId",
                principalTable: "LabInstitution",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_Visits_VisitId",
                table: "LabTests",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_LabInstitution_LabInstitutionId",
                table: "LabTests");

            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_Visits_VisitId",
                table: "LabTests");

            migrationBuilder.DropTable(
                name: "LabInstitution");

            migrationBuilder.DropIndex(
                name: "IX_LabTests_LabInstitutionId",
                table: "LabTests");

            migrationBuilder.DropIndex(
                name: "IX_LabTests_VisitId",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "LabInstitutionId",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "LabTests");

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "Visits",
                type: "decimal(4,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HeartRate",
                table: "Visits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BloodPressure",
                table: "Visits",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
