using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreateDocumentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDocument",
                table: "Contracts");

            migrationBuilder.AddColumn<Guid>(
                name: "AdditionalDocumentId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AdditionalDocumentId",
                table: "Contracts",
                column: "AdditionalDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Document_AdditionalDocumentId",
                table: "Contracts",
                column: "AdditionalDocumentId",
                principalTable: "Document",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Document_AdditionalDocumentId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_AdditionalDocumentId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AdditionalDocumentId",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalDocument",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
