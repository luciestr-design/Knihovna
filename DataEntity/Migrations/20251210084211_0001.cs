using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataEntity.Migrations
{
    /// <inheritdoc />
    public partial class _0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Knihy",
                columns: table => new
                {
                    KnihaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazev = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Zanr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RokVydani = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CelkemKusu = table.Column<int>(type: "int", nullable: false),
                    KusuKDispozici = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DatumVytvoreni = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knihy", x => x.KnihaId);
                });

            migrationBuilder.CreateTable(
                name: "Uzivatele",
                columns: table => new
                {
                    UzivatelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jmeno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prijmeni = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Dluh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DatumVytvoreni = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzivatele", x => x.UzivatelId);
                });

            migrationBuilder.CreateTable(
                name: "Vypujcky",
                columns: table => new
                {
                    VypujckaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzivatelId = table.Column<int>(type: "int", nullable: false),
                    KnihaId = table.Column<int>(type: "int", nullable: false),
                    DatumPujceni = table.Column<DateTime>(type: "date", nullable: false),
                    DatumSplatnosti = table.Column<DateTime>(type: "date", nullable: false),
                    DatumVraceni = table.Column<DateTime>(type: "date", nullable: true),
                    Stav = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DatumVytvoreni = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vypujcky", x => x.VypujckaId);
                    table.ForeignKey(
                        name: "FK_Vypujcky_Knihy_KnihaId",
                        column: x => x.KnihaId,
                        principalTable: "Knihy",
                        principalColumn: "KnihaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vypujcky_Uzivatele_UzivatelId",
                        column: x => x.UzivatelId,
                        principalTable: "Uzivatele",
                        principalColumn: "UzivatelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vypujcky_KnihaId",
                table: "Vypujcky",
                column: "KnihaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vypujcky_UzivatelId",
                table: "Vypujcky",
                column: "UzivatelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vypujcky");

            migrationBuilder.DropTable(
                name: "Knihy");

            migrationBuilder.DropTable(
                name: "Uzivatele");
        }
    }
}
