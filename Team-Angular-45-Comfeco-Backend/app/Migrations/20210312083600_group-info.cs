using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class groupinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGroupLeader",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "5c1d24e6-4a1a-415f-82cd-c5222d177ee1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupLeader",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "aae958be-0554-48d6-a221-1535e34181be");
        }
    }
}
