using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class defaultbadge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_SpecialtyId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "3941e0c1-6fbf-4ebb-af22-308f0dbe489c");

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "BadgeIcon", "Description", "Instructions", "Name" },
                values: new object[] { 2, null, "Esta persona es muy competitiva", "Inscribete en tu primer evento", "Concursante" });

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "BadgeIcon", "Description", "Instructions", "Name" },
                values: new object[] { 1, null, "Esta persona es muy sociable", "Para obtener esta insignia actualiza los datos de tu perfil", "Sociable" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_SpecialtyId",
                table: "AspNetUsers",
                column: "SpecialtyId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_SpecialtyId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "8b425617-41fd-4a5f-8ab9-7647c17c0b78");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_SpecialtyId",
                table: "AspNetUsers",
                column: "SpecialtyId",
                principalTable: "Areas",
                principalColumn: "Id");
        }
    }
}
