using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotK_TechShop.Migrations
{
    /// <inheritdoc />
    public partial class MyDbContext_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(455)",
                maxLength: 455,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 455);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Image",
                table: "Products",
                type: "float",
                maxLength: 455,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(455)",
                oldMaxLength: 455);
        }
    }
}
