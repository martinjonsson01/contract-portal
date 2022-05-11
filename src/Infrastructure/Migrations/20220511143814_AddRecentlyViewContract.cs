using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddRecentlyViewContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractUser");

            migrationBuilder.CreateTable(
                name: "RecentlyViewedContracts",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastViewed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentlyViewedContracts", x => new { x.ContractId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RecentlyViewedContracts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentlyViewedContracts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecentlyViewedContracts_UserId",
                table: "RecentlyViewedContracts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecentlyViewedContracts");

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
    }
}
