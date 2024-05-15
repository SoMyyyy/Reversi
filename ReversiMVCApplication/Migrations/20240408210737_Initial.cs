using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMVCApplication.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameState = table.Column<int>(type: "int", nullable: true),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speler1Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speler2Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AanDeBeurt = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spelers",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AantalGewonnen = table.Column<int>(type: "int", nullable: false),
                    AantalVerloren = table.Column<int>(type: "int", nullable: false),
                    AantalGelijk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelers", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spel");

            migrationBuilder.DropTable(
                name: "Spelers");
        }
    }
}
