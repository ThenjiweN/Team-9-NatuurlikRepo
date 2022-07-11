using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatuurlikBase.Migrations
{
    public partial class amendOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourierName",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Suburb",
                table: "Order",
                newName: "SuburbId");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Order",
                newName: "ProvinceId");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Order",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Order",
                newName: "CityId");

            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InclusiveVAT",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "VATId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CityId",
                table: "Order",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CountryId",
                table: "Order",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CourierId",
                table: "Order",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProvinceId",
                table: "Order",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SuburbId",
                table: "Order",
                column: "SuburbId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_VATId",
                table: "Order",
                column: "VATId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_City_CityId",
                table: "Order",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Country_CountryId",
                table: "Order",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Province_ProvinceId",
                table: "Order",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Suburb_SuburbId",
                table: "Order",
                column: "SuburbId",
                principalTable: "Suburb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_VAT_VATId",
                table: "Order",
                column: "VATId",
                principalTable: "VAT",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_City_CityId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Country_CountryId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Province_ProvinceId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Suburb_SuburbId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_VAT_VATId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CityId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CountryId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CourierId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProvinceId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SuburbId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_VATId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "InclusiveVAT",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "VATId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "SuburbId",
                table: "Order",
                newName: "Suburb");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Order",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Order",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Order",
                newName: "City");

            migrationBuilder.AddColumn<string>(
                name: "CourierName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
