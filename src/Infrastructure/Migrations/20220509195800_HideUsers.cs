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
                newName: "UserFavoriteContracts");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "UserFavoriteContracts",
                newName: "FavoritesId");

            migrationBuilder.RenameColumn(
                name: "ContractsId",
                table: "UserFavoriteContracts",
                newName: "FavoriteUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserContracts_UsersId",
                table: "UserFavoriteContracts",
                newName: "IX_UserFavoriteContracts_FavoritesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteContracts",
                table: "UserFavoriteContracts",
                columns: new[] { "FavoriteUsersId", "FavoritesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteContracts_Contracts_FavoritesId",
                table: "UserFavoriteContracts",
                column: "FavoritesId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteContracts_Users_FavoriteUsersId",
                table: "UserFavoriteContracts",
                column: "FavoriteUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteContracts_Contracts_FavoritesId",
                table: "UserFavoriteContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteContracts_Users_FavoriteUsersId",
                table: "UserFavoriteContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteContracts",
                table: "UserFavoriteContracts");

            migrationBuilder.RenameTable(
                name: "UserFavoriteContracts",
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
                name: "IX_UserFavoriteContracts_FavoritesId",
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
