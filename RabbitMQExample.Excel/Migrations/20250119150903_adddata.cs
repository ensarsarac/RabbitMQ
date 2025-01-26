using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RabbitMQExample.Excel.Migrations
{
    public partial class adddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8098d13c-292a-4863-9ba7-bf9d0ae4bacb", 0, "9eb2a46e-e4d0-4fb8-a80c-12d4c92d7ccc", "ensar.src94@gmail.com", false, false, null, "ENSAR.SRC94@GMAIL.COM", null, "AQAAAAEAACcQAAAAEDX16o4g4c2Z2AneONaE92s67PrUR3QlxChK/UqO3VfiG5BR/3eRAl8czwg7Xq3H0Q==", null, false, "0e73a861-1075-4b81-a764-bcd5184053a7", false, "ensarsarac" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8098d13c-292a-4863-9ba7-bf9d0ae4bacb");
        }
    }
}
