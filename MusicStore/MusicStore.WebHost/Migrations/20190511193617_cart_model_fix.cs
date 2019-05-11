using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicStore.WebHost.Migrations
{
    public partial class cart_model_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "Cart",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_AlbumId",
                table: "Cart",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Album_AlbumId",
                table: "Cart",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Album_AlbumId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_AlbumId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Cart");
        }
    }
}
