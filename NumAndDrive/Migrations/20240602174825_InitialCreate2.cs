using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NumAndDrive.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserType",
                columns: new[] { "UserTypeId", "TypeName" },
                values: new object[] { 11, "Nouveau-elle venu-e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserType",
                keyColumn: "UserTypeId",
                keyValue: 11);
        }
    }
}
