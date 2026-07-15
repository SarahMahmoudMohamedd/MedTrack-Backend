using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedTrack.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddLabSeedingAndContextUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_LabInstitution_LabInstitutionId",
                table: "LabTests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabInstitution",
                table: "LabInstitution");

            migrationBuilder.RenameTable(
                name: "LabInstitution",
                newName: "LabInstitutions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabInstitutions",
                table: "LabInstitutions",
                column: "Id");

            migrationBuilder.InsertData(
                table: "LabInstitutions",
                columns: new[] { "Id", "Address", "CreatedAt", "DirectorName", "Email", "EstablishedDate", "InstitutionType", "LicenseNumber", "Name", "PasswordHash", "PhoneNumber", "RegistrationNumber", "UpdatedAt", "Website" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), "", new DateTime(2026, 6, 11, 13, 45, 41, 131, DateTimeKind.Utc).AddTicks(1507), "", "demo.lab@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "", "Elmo5tabar Labs", "$2a$11$EvXNshA4lA9lB.68F0X3vO3K3K1oZIn79eMkWy8S0W8wD72K5q6vG", "01234567890", "", null, "" });

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_LabInstitutions_LabInstitutionId",
                table: "LabTests",
                column: "LabInstitutionId",
                principalTable: "LabInstitutions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_LabInstitutions_LabInstitutionId",
                table: "LabTests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabInstitutions",
                table: "LabInstitutions");

            migrationBuilder.DeleteData(
                table: "LabInstitutions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.RenameTable(
                name: "LabInstitutions",
                newName: "LabInstitution");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabInstitution",
                table: "LabInstitution",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_LabInstitution_LabInstitutionId",
                table: "LabTests",
                column: "LabInstitutionId",
                principalTable: "LabInstitution",
                principalColumn: "Id");
        }

    }
}
