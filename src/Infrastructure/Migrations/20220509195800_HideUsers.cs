using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class HideUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContracts_Contracts_ContractsId",
                table: "UserContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContracts_Users_UsersId",
                table: "UserContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContracts",
                table: "UserContracts");

            migrationBuilder.RenameTable(
                name: "UserContracts",
                newName: "ContractUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ContractUser",
                newName: "FavoritesId");

            migrationBuilder.RenameColumn(
                name: "ContractsId",
                table: "ContractUser",
                newName: "FavoriteUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserContracts_UsersId",
                table: "ContractUser",
                newName: "IX_ContractUser_FavoritesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractUser",
                table: "ContractUser",
                columns: new[] { "FavoriteUsersId", "FavoritesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUser_Contracts_FavoritesId",
                table: "ContractUser",
                column: "FavoritesId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUser_Users_FavoriteUsersId",
                table: "ContractUser",
                column: "FavoriteUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractUser_Contracts_FavoritesId",
                table: "ContractUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractUser_Users_FavoriteUsersId",
                table: "ContractUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractUser",
                table: "ContractUser");

            migrationBuilder.RenameTable(
                name: "ContractUser",
                newName: "UserContracts");

            migrationBuilder.RenameColumn(
                name: "FavoritesId",
                table: "UserContracts",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "FavoriteUsersId",
                table: "UserContracts",
                newName: "ContractsId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractUser_FavoritesId",
                table: "UserContracts",
                newName: "IX_UserContracts_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContracts",
                table: "UserContracts",
                columns: new[] { "ContractsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserContracts_Contracts_ContractsId",
                table: "UserContracts",
                column: "ContractsId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContracts_Users_UsersId",
                table: "UserContracts",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
