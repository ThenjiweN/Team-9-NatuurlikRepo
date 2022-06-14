using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatuurlikBase.Migrations
{
    public partial class inventoriesTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InventoryTransactionId",
                table: "InventoryItemTransaction",
                newName: "InventoryItemTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InventoryItemTransactionId",
                table: "InventoryItemTransaction",
                newName: "InventoryTransactionId");
        }
    }
}
