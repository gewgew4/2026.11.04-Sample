using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PadelClub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canchas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Número = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Techada = table.Column<bool>(type: "bit", nullable: false),
                    PrecioPorTurno = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Habilitada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canchas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dni = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanchaId = table.Column<int>(type: "int", nullable: false),
                    SocioId = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Canchas_CanchaId",
                        column: x => x.CanchaId,
                        principalTable: "Canchas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Canchas",
                columns: new[] { "Id", "Habilitada", "Número", "PrecioPorTurno", "Techada", "Tipo" },
                values: new object[,]
                {
                    { 1, true, 1, 18000m, true, "Cristal" },
                    { 2, true, 2, 15000m, false, "Cristal" },
                    { 3, true, 3, 16000m, true, "Muro" },
                    { 4, true, 4, 14000m, false, "Muro" }
                });

            migrationBuilder.InsertData(
                table: "Socios",
                columns: new[] { "Id", "Activo", "Apellido", "Dni", "Email", "FechaAlta", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Pérez", "30111222", "ana.perez@padelclub.test", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana" },
                    { 2, true, "Gómez", "28999111", "luis.gomez@padelclub.test", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luis" },
                    { 3, true, "Díaz", "33444555", "maria.diaz@padelclub.test", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "María" },
                    { 4, true, "Ruiz", "25666777", "juan.ruiz@padelclub.test", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Juan" },
                    { 5, false, "Sosa", "20123456", "pedro.sosa@padelclub.test", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pedro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Canchas_Número",
                table: "Canchas",
                column: "Número",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_CanchaId",
                table: "Reservas",
                column: "CanchaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_SocioId",
                table: "Reservas",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_Dni",
                table: "Socios",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Socios_Email",
                table: "Socios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Canchas");

            migrationBuilder.DropTable(
                name: "Socios");
        }
    }
}
