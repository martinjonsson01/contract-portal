using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddManyToManyRecents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_UserId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contracts");

            migrationBuilder.CreateTable(
                name: "ContractUser",
                columns: table => new
                {
                    RecentOfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecentlyViewContractsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractUser", x => new { x.RecentOfId, x.RecentlyViewContractsId });
                    table.ForeignKey(
                        name: "FK_ContractUser_Contracts_RecentlyViewContractsId",
                        column: x => x.RecentlyViewContractsId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractUser_Users_RecentOfId",
                        column: x => x.RecentOfId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractUser_RecentlyViewContractsId",
                table: "ContractUser",
                column: "RecentlyViewContractsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractUser");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_UserId",
                table: "Contracts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
