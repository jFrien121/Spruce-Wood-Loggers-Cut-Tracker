using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Spruce_Wood_Loggers_ERP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timeProcessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    thickness = table.Column<double>(type: "double precision", nullable: false),
                    width = table.Column<double>(type: "double precision", nullable: false),
                    length = table.Column<double>(type: "double precision", nullable: false),
                    grade = table.Column<string>(type: "text", nullable: false),
                    numPieces = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CutLengths",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    length = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutLengths", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CutSizes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    thickness = table.Column<double>(type: "double precision", nullable: false),
                    width = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutSizes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LiftResults",
                columns: table => new
                {
                    thickness = table.Column<double>(type: "double precision", nullable: false),
                    width = table.Column<double>(type: "double precision", nullable: false),
                    length = table.Column<double>(type: "double precision", nullable: false),
                    numberOfLifts = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StandardNumPieces",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numPieces = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardNumPieces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StandardSizeRelationships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StandardNumPiecesId = table.Column<int>(type: "integer", nullable: false),
                    CutSizeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardSizeRelationships", x => x.id);
                    table.ForeignKey(
                        name: "FK_StandardSizeRelationships_CutSizes_CutSizeId",
                        column: x => x.CutSizeId,
                        principalTable: "CutSizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardSizeRelationships_StandardNumPieces_StandardNumPiec~",
                        column: x => x.StandardNumPiecesId,
                        principalTable: "StandardNumPieces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CutLengths_length",
                table: "CutLengths",
                column: "length",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CutSizes_thickness_width",
                table: "CutSizes",
                columns: new[] { "thickness", "width" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StandardNumPieces_numPieces",
                table: "StandardNumPieces",
                column: "numPieces",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StandardSizeRelationships_CutSizeId",
                table: "StandardSizeRelationships",
                column: "CutSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardSizeRelationships_StandardNumPiecesId_CutSizeId",
                table: "StandardSizeRelationships",
                columns: new[] { "StandardNumPiecesId", "CutSizeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bundles");

            migrationBuilder.DropTable(
                name: "CutLengths");

            migrationBuilder.DropTable(
                name: "LiftResults");

            migrationBuilder.DropTable(
                name: "StandardSizeRelationships");

            migrationBuilder.DropTable(
                name: "CutSizes");

            migrationBuilder.DropTable(
                name: "StandardNumPieces");
        }
    }
}
