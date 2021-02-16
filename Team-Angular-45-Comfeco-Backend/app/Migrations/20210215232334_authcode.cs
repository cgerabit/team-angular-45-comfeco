using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class authcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalLoginValidsRedirectUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLoginValidsRedirectUrl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthenticationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthenticationCodes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExternalLoginValidsRedirectUrl",
                columns: new[] { "Id", "Url" },
                values: new object[] { 1, "http://localhost:4200/external-signin-callback" });

            migrationBuilder.InsertData(
                table: "ExternalLoginValidsRedirectUrl",
                columns: new[] { "Id", "Url" },
                values: new object[] { 2, "https://localhost:4200/external-signin-callback" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalLoginValidsRedirectUrl");

            migrationBuilder.DropTable(
                name: "UserAuthenticationCodes");
        }
    }
}
