using Microsoft.EntityFrameworkCore.Migrations;

namespace BackendComfeco.Migrations
{
    public partial class persistentlogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthenticationCodes",
                table: "UserAuthenticationCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalLoginValidsRedirectUrl",
                table: "ExternalLoginValidsRedirectUrl");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAuthenticationCodes");

            migrationBuilder.RenameTable(
                name: "ExternalLoginValidsRedirectUrl",
                newName: "ExternalLoginValidRedirectUrls");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserAuthenticationCodes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ExternalLoginValidRedirectUrls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthenticationCodes",
                table: "UserAuthenticationCodes",
                column: "Token");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalLoginValidRedirectUrls",
                table: "ExternalLoginValidRedirectUrls",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PersistentLoginValidRedirectUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistentLoginValidRedirectUrls", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PersistentLoginValidRedirectUrls",
                columns: new[] { "Id", "Url" },
                values: new object[] { 1, "https://localhost:4200/auth/persistent-signin-callback" });

            migrationBuilder.InsertData(
                table: "PersistentLoginValidRedirectUrls",
                columns: new[] { "Id", "Url" },
                values: new object[] { 2, "http://localhost:4200/auth/persistent-signin-callback" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistentLoginValidRedirectUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAuthenticationCodes",
                table: "UserAuthenticationCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalLoginValidRedirectUrls",
                table: "ExternalLoginValidRedirectUrls");

            migrationBuilder.RenameTable(
                name: "ExternalLoginValidRedirectUrls",
                newName: "ExternalLoginValidsRedirectUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserAuthenticationCodes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserAuthenticationCodes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ExternalLoginValidsRedirectUrl",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAuthenticationCodes",
                table: "UserAuthenticationCodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalLoginValidsRedirectUrl",
                table: "ExternalLoginValidsRedirectUrl",
                column: "Id");
        }
    }
}
