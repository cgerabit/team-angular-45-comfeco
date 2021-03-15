using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class groupbadge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "270a7e9b-b460-46fa-ab8e-1f956fddb2ee");

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "BadgeIcon", "Description", "Instructions", "Name" },
                values: new object[] { 3, null, "Esta persona le encanta compartir en comunidad", "Unete a tu primer grupo", "Grupero" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "a3ff4e8e-9d64-4f84-81dd-ff66e1dff2ad");
        }
    }
}
