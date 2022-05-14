using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractUser_Users_FavoriteUsersId",
                table: "ContractUser");

            migrationBuilder.RenameColumn(
                name: "FavoriteUsersId",
                table: "ContractUser",
                newName: "FavoritedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUser_Users_FavoritedById",
                table: "ContractUser",
                column: "FavoritedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractUser_Users_FavoritedById",
                table: "ContractUser");

            migrationBuilder.RenameColumn(
                name: "FavoritedById",
                table: "ContractUser",
                newName: "FavoriteUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUser_Users_FavoriteUsersId",
                table: "ContractUser",
                column: "FavoriteUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
