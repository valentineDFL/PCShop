using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class testCRCP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryEntityCharacteristicEntity");

            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.CreateTable(
                name: "CharacteristicPatterns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicPatterns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntityCharacteristicPatternEntity",
                columns: table => new
                {
                    CategoryEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacteristicPatternsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntityCharacteristicPatternEntity", x => new { x.CategoryEntityId, x.CharacteristicPatternsId });
                    table.ForeignKey(
                        name: "FK_CategoryEntityCharacteristicPatternEntity_Categories_Catego~",
                        column: x => x.CategoryEntityId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryEntityCharacteristicPatternEntity_CharacteristicPat~",
                        column: x => x.CharacteristicPatternsId,
                        principalTable: "CharacteristicPatterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacteristicRealizing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CharacteristicPatternId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicRealizing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacteristicRealizing_CharacteristicPatterns_Characterist~",
                        column: x => x.CharacteristicPatternId,
                        principalTable: "CharacteristicPatterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_CategoryEntityCharacteristicPatternEntity_CharacteristicPat~",
                table: "CategoryEntityCharacteristicPatternEntity",
                column: "CharacteristicPatternsId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicRealizeEntityProductEntity_ProductEntityId",
                table: "CharacteristicRealizeEntityProductEntity",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicRealizing_CharacteristicPatternId",
                table: "CharacteristicRealizing",
                column: "CharacteristicPatternId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryEntityCharacteristicPatternEntity");

            migrationBuilder.DropTable(
                name: "CharacteristicRealizeEntityProductEntity");

            migrationBuilder.DropTable(
                name: "CharacteristicRealizing");

            migrationBuilder.DropTable(
                name: "CharacteristicPatterns");

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Value = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryEntityCharacteristicEntity",
                columns: table => new
                {
                    CategoryEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacteristicsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntityCharacteristicEntity", x => new { x.CategoryEntityId, x.CharacteristicsId });
                    table.ForeignKey(
                        name: "FK_CategoryEntityCharacteristicEntity_Categories_CategoryEntit~",
                        column: x => x.CategoryEntityId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryEntityCharacteristicEntity_Characteristics_Characte~",
                        column: x => x.CharacteristicsId,
                        principalTable: "Characteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntityCharacteristicEntity_CharacteristicsId",
                table: "CategoryEntityCharacteristicEntity",
                column: "CharacteristicsId");
        }
    }
}
