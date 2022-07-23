using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatuurlikBase.Migrations
{
    public partial class paymentreminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentReminderId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentReminder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReminder", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentReminderId",
                table: "Order",
                column: "PaymentReminderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PaymentReminder_PaymentReminderId",
                table: "Order",
                column: "PaymentReminderId",
                principalTable: "PaymentReminder",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PaymentReminder_PaymentReminderId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "PaymentReminder");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentReminderId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentReminderId",
                table: "Order");
        }
    }
}
