using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RabbitMQExample.Excel.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "UserFiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8098d13c-292a-4863-9ba7-bf9d0ae4bacb",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e66d933d-bb72-41bc-b1ac-20fdf044f030", "ENSARSARAC", "AQAAAAEAACcQAAAAELBnhe4ovoT4PIsxbsLegqzNLncwPeG8CWRwZuWEEYhKmXt043SMn+3VEBSEL5lZBA==", "abaac262-5b03-4a93-9617-c393b3ebe673" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "UserFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8098d13c-292a-4863-9ba7-bf9d0ae4bacb",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9eb2a46e-e4d0-4fb8-a80c-12d4c92d7ccc", null, "AQAAAAEAACcQAAAAEDX16o4g4c2Z2AneONaE92s67PrUR3QlxChK/UqO3VfiG5BR/3eRAl8czwg7Xq3H0Q==", "0e73a861-1075-4b81-a764-bcd5184053a7" });
        }
    }
}
