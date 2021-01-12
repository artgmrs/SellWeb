using Microsoft.EntityFrameworkCore.Migrations;

namespace SellWeb.Data.Migrations
{
    public partial class UpdatesInProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Produtos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Produtos",
                type: "varchar(200)",
                nullable: true);
        }
    }
}
