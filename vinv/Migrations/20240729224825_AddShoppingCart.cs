using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vinv.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ProductStocks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_ShoppingCartId",
                table: "ProductStocks",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStocks_ShoppingCarts_ShoppingCartId",
                table: "ProductStocks",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStocks_ShoppingCarts_ShoppingCartId",
                table: "ProductStocks");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_ShoppingCartId",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ProductStocks");
        }
    }
}
