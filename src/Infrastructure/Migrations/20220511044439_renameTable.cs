using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteContracts_Users_FavoriteUsersId",
                table: "UserFavoriteContracts");

            migrationBuilder.RenameColumn(
                name: "FavoriteUsersId",
                table: "UserFavoriteContracts",
                newName: "FavoritedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteContracts_Users_FavoritedById",
                table: "UserFavoriteContracts",
                column: "FavoritedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteContracts_Users_FavoritedById",
                table: "UserFavoriteContracts");

            migrationBuilder.RenameColumn(
                name: "FavoritedById",
                table: "UserFavoriteContracts",
                newName: "FavoriteUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteContracts_Users_FavoriteUsersId",
                table: "UserFavoriteContracts",
                column: "FavoriteUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
