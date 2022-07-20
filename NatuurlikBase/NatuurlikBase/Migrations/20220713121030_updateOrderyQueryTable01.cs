using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatuurlikBase.Migrations
{
    public partial class updateOrderyQueryTable01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadEvidenceUrl",
                table: "OrderQuery",
                newName: "QueryStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QueryStatus",
                table: "OrderQuery",
                newName: "UploadEvidenceUrl");
        }
    }
}
