using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class groupsdescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "aae958be-0554-48d6-a221-1535e34181be");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groups");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "3d02892b-77a5-460a-b650-58fad05d4286");
        }
    }
}
