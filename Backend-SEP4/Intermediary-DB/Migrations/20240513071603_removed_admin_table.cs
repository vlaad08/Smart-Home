using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBComm.Migrations
{
    /// <inheritdoc />
    public partial class removed_admin_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_member_admin_AdminId",
                table: "member");

            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropIndex(
                name: "IX_member_AdminId",
                table: "member");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "member");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "member",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "member");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "member",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    HomeId = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_admin_home_HomeId",
                        column: x => x.HomeId,
                        principalTable: "home",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_member_AdminId",
                table: "member",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_admin_HomeId",
                table: "admin",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_member_admin_AdminId",
                table: "member",
                column: "AdminId",
                principalTable: "admin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
