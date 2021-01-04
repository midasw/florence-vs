using Microsoft.EntityFrameworkCore.Migrations;

namespace BackOffice_ASP.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Country_CountryCode",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Countries_CountryCode",
                table: "Users",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Countries_CountryCode",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Country");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Country_CountryCode",
                table: "Users",
                column: "CountryCode",
                principalTable: "Country",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
