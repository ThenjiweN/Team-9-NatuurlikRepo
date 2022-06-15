using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatuurlikBase.Migrations
{
    public partial class writeoffs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryWriteOff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    writeOffQuantity = table.Column<int>(type: "int", nullable: false),
                    writeOffDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    writeOffReasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryWriteOff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryWriteOff_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryWriteOff_WriteOffReason_writeOffReasonId",
                        column: x => x.writeOffReasonId,
                        principalTable: "WriteOffReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductWriteOff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    writeOffQuantity = table.Column<int>(type: "int", nullable: false),
                    writeOffDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    writeOffReasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWriteOff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductWriteOff_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductWriteOff_WriteOffReason_writeOffReasonId",
                        column: x => x.writeOffReasonId,
                        principalTable: "WriteOffReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryWriteOff_InventoryItemId",
                table: "InventoryWriteOff",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryWriteOff_writeOffReasonId",
                table: "InventoryWriteOff",
                column: "writeOffReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWriteOff_ProductId",
                table: "ProductWriteOff",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWriteOff_writeOffReasonId",
                table: "ProductWriteOff",
                column: "writeOffReasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryWriteOff");

            migrationBuilder.DropTable(
                name: "ProductWriteOff");
        }
    }
}
