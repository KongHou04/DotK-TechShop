using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotK_TechShop.Migrations
{
    /// <inheritdoc />
    public partial class MyDbContext_04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Products",
                newName: "Storage");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(455)",
                maxLength: 455,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(455)",
                oldMaxLength: 455);

            migrationBuilder.AddColumn<int>(
                name: "RAM",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RAM",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Storage",
                table: "Products",
                newName: "CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(455)",
                maxLength: 455,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(455)",
                oldMaxLength: 455,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Products",
                type: "nvarchar(455)",
                maxLength: 455,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
