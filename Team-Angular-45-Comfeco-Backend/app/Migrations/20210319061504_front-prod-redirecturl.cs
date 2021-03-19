using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class frontprodredirecturl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "9bb791b2-0a47-4c3e-891c-f7ae8872106a");

            migrationBuilder.InsertData(
                table: "ExternalLoginValidRedirectUrls",
                columns: new[] { "Id", "Url" },
                values: new object[,]
                {
                    { 3, "http://team45.comfeco.cristiangerani.com/auth/external-signin-callback" },
                    { 4, "https://team45.comfeco.cristiangerani.com/auth/external-signin-callback" }
                });

            migrationBuilder.InsertData(
                table: "PersistentLoginValidRedirectUrls",
                columns: new[] { "Id", "Url" },
                values: new object[,]
                {
                    { 3, "https://team45.comfeco.cristiangerani.com/auth/external-signin-callback" },
                    { 4, "http://team45.comfeco.cristiangerani.com/auth/external-signin-callback" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExternalLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ExternalLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PersistentLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PersistentLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "270a7e9b-b460-46fa-ab8e-1f956fddb2ee");
        }
    }
}
