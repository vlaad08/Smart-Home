using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBComm.Migrations
{
    /// <inheritdoc />
    public partial class adding_nullable_home : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_member_home_HomeId",
                table: "member");

            migrationBuilder.AlterColumn<string>(
                name: "HomeId",
                table: "member",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_member_home_HomeId",
                table: "member",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_member_home_HomeId",
                table: "member");

            migrationBuilder.AlterColumn<string>(
                name: "HomeId",
                table: "member",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_member_home_HomeId",
                table: "member",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
