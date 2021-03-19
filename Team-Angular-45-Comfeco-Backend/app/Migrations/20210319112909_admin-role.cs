using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class adminrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "b699a2ad-19e3-4b11-bc19-4523374e4d88");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88d3be36-a8aa-422c-b6a2-99b98a042fed", "039547ba-4ab1-4d66-b78c-5a6914172ec5", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88d3be36-a8aa-422c-b6a2-99b98a042fed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "9bb791b2-0a47-4c3e-891c-f7ae8872106a");
        }
    }
}
