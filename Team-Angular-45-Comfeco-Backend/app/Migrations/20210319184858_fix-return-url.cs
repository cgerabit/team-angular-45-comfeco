using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class fixreturnurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "d7dd16c7-cb9e-4935-9ac9-7b71209f3729");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88d3be36-a8aa-422c-b6a2-99b98a042fed",
                column: "ConcurrencyStamp",
                value: "94587ff2-9bf1-45df-b254-cdcb0b39e586");

            migrationBuilder.UpdateData(
                table: "PersistentLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 3,
                column: "Url",
                value: "https://team45.comfeco.cristiangerani.com/auth/persistent-signin-callback");

            migrationBuilder.UpdateData(
                table: "PersistentLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 4,
                column: "Url",
                value: "http://team45.comfeco.cristiangerani.com/auth/persistent-signin-callback");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a8af04b-0405-4cd2-bc20-d59433235153",
                column: "ConcurrencyStamp",
                value: "b699a2ad-19e3-4b11-bc19-4523374e4d88");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88d3be36-a8aa-422c-b6a2-99b98a042fed",
                column: "ConcurrencyStamp",
                value: "039547ba-4ab1-4d66-b78c-5a6914172ec5");

            migrationBuilder.UpdateData(
                table: "PersistentLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 3,
                column: "Url",
                value: "https://team45.comfeco.cristiangerani.com/auth/external-signin-callback");

            migrationBuilder.UpdateData(
                table: "PersistentLoginValidRedirectUrls",
                keyColumn: "Id",
                keyValue: 4,
                column: "Url",
                value: "http://team45.comfeco.cristiangerani.com/auth/external-signin-callback");
        }
    }
}
