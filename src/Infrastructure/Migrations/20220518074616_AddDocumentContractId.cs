using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddDocumentContractId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Document_AdditionalDocumentId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_AdditionalDocumentId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AdditionalDocumentId",
                table: "Contracts");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Document",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Document_ContractId",
                table: "Document",
                column: "ContractId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Contracts_ContractId",
                table: "Document",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Contracts_ContractId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_ContractId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Document");

            migrationBuilder.AddColumn<Guid>(
                name: "AdditionalDocumentId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

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
    }
}
