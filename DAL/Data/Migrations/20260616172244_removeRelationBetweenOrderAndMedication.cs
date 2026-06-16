using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeRelationBetweenOrderAndMedication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBaskets_Medications_MedicationId",
                table: "CustomerBaskets");

            migrationBuilder.DropIndex(
                name: "IX_CustomerBaskets_MedicationId",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "MedicationId",
                table: "CustomerBaskets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicationId",
                table: "CustomerBaskets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBaskets_MedicationId",
                table: "CustomerBaskets",
                column: "MedicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBaskets_Medications_MedicationId",
                table: "CustomerBaskets",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
