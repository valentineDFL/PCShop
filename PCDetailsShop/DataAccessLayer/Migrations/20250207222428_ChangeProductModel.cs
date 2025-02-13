using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacteristicRealizeEntityProductEntity");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "StockAvailability",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CharacteristicRealizationEntityProductEntity",
                columns: table => new
                {
                    CharacteristicsRealizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicRealizationEntityProductEntity", x => new { x.CharacteristicsRealizationId, x.ProductEntityId });
                    table.ForeignKey(
                        name: "FK_CharacteristicRealizationEntityProductEntity_Characteristic~",
                        column: x => x.CharacteristicsRealizationId,
                        principalTable: "CharacteristicRealizing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacteristicRealizationEntityProductEntity_Products_Produ~",
                        column: x => x.ProductEntityId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicRealizationEntityProductEntity_ProductEntityId",
                table: "CharacteristicRealizationEntityProductEntity",
                column: "ProductEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacteristicRealizationEntityProductEntity");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockAvailability",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "CharacteristicRealizeEntityProductEntity",
                columns: table => new
                {
                    CharacteristicsRealizingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicRealizeEntityProductEntity", x => new { x.CharacteristicsRealizingId, x.ProductEntityId });
                    table.ForeignKey(
                        name: "FK_CharacteristicRealizeEntityProductEntity_CharacteristicReal~",
                        column: x => x.CharacteristicsRealizingId,
                        principalTable: "CharacteristicRealizing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacteristicRealizeEntityProductEntity_Products_ProductEn~",
                        column: x => x.ProductEntityId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicRealizeEntityProductEntity_ProductEntityId",
                table: "CharacteristicRealizeEntityProductEntity",
                column: "ProductEntityId");
        }
    }
}
