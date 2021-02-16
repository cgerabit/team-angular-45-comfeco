using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class changedebugcallbacks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalLoginValidsRedirectUrl",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "http://localhost:4200/auth/external-signin-callback");

            migrationBuilder.UpdateData(
                table: "ExternalLoginValidsRedirectUrl",
                keyColumn: "Id",
                keyValue: 2,
                column: "Url",
                value: "https://localhost:4200/auth/external-signin-callback");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalLoginValidsRedirectUrl",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "http://localhost:4200/external-signin-callback");

            migrationBuilder.UpdateData(
                table: "ExternalLoginValidsRedirectUrl",
                keyColumn: "Id",
                keyValue: 2,
                column: "Url",
                value: "https://localhost:4200/external-signin-callback");
        }
    }
}
