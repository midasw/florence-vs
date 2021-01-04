using Microsoft.EntityFrameworkCore.Migrations;

namespace BackOffice_ASP.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HouseNumberSuffix",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "StreetName",
                table: "Users",
                newName: "StreetAddress");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryCode",
                table: "Users",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Country_CountryCode",
                table: "Users",
                column: "CountryCode",
                principalTable: "Country",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Country_CountryCode",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Users_CountryCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "Users",
                newName: "StreetName");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HouseNumber",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumberSuffix",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
