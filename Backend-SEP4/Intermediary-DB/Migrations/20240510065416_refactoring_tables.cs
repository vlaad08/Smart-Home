using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBComm.Migrations
{
    /// <inheritdoc />
    public partial class refactoring_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Homes_HomeId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Homes_HomeId",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_HumidityReadings_Rooms_RoomId",
                table: "HumidityReadings");

            migrationBuilder.DropForeignKey(
                name: "FK_LightReadings_Rooms_RoomId",
                table: "LightReadings");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Admins_AdminId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Homes_HomeId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Homes_HomeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Homes_HomeId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TemperatureReadings_Rooms_RoomId",
                table: "TemperatureReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemperatureReadings",
                table: "TemperatureReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LightReadings",
                table: "LightReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HumidityReadings",
                table: "HumidityReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homes",
                table: "Homes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doors",
                table: "Doors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "TemperatureReadings",
                newName: "temperature_reading");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "room");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notification");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "member");

            migrationBuilder.RenameTable(
                name: "LightReadings",
                newName: "light_reading");

            migrationBuilder.RenameTable(
                name: "HumidityReadings",
                newName: "humidity_reading");

            migrationBuilder.RenameTable(
                name: "Homes",
                newName: "home");

            migrationBuilder.RenameTable(
                name: "Doors",
                newName: "door");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "admin");

            migrationBuilder.RenameIndex(
                name: "IX_TemperatureReadings_RoomId",
                table: "temperature_reading",
                newName: "IX_temperature_reading_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_HomeId",
                table: "room",
                newName: "IX_room_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_HomeId",
                table: "notification",
                newName: "IX_notification_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_HomeId",
                table: "member",
                newName: "IX_member_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_AdminId",
                table: "member",
                newName: "IX_member_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_LightReadings_RoomId",
                table: "light_reading",
                newName: "IX_light_reading_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_HumidityReadings_RoomId",
                table: "humidity_reading",
                newName: "IX_humidity_reading_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Doors_HomeId",
                table: "door",
                newName: "IX_door_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_HomeId",
                table: "admin",
                newName: "IX_admin_HomeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_temperature_reading",
                table: "temperature_reading",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_room",
                table: "room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notification",
                table: "notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_member",
                table: "member",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_light_reading",
                table: "light_reading",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_humidity_reading",
                table: "humidity_reading",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_home",
                table: "home",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_door",
                table: "door",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admin",
                table: "admin",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_admin_home_HomeId",
                table: "admin",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_door_home_HomeId",
                table: "door",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_humidity_reading_room_RoomId",
                table: "humidity_reading",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_light_reading_room_RoomId",
                table: "light_reading",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_member_admin_AdminId",
                table: "member",
                column: "AdminId",
                principalTable: "admin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_member_home_HomeId",
                table: "member",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_home_HomeId",
                table: "notification",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_room_home_HomeId",
                table: "room",
                column: "HomeId",
                principalTable: "home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_temperature_reading_room_RoomId",
                table: "temperature_reading",
                column: "RoomId",
                principalTable: "room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admin_home_HomeId",
                table: "admin");

            migrationBuilder.DropForeignKey(
                name: "FK_door_home_HomeId",
                table: "door");

            migrationBuilder.DropForeignKey(
                name: "FK_humidity_reading_room_RoomId",
                table: "humidity_reading");

            migrationBuilder.DropForeignKey(
                name: "FK_light_reading_room_RoomId",
                table: "light_reading");

            migrationBuilder.DropForeignKey(
                name: "FK_member_admin_AdminId",
                table: "member");

            migrationBuilder.DropForeignKey(
                name: "FK_member_home_HomeId",
                table: "member");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_home_HomeId",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_room_home_HomeId",
                table: "room");

            migrationBuilder.DropForeignKey(
                name: "FK_temperature_reading_room_RoomId",
                table: "temperature_reading");

            migrationBuilder.DropPrimaryKey(
                name: "PK_temperature_reading",
                table: "temperature_reading");

            migrationBuilder.DropPrimaryKey(
                name: "PK_room",
                table: "room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notification",
                table: "notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_member",
                table: "member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_light_reading",
                table: "light_reading");

            migrationBuilder.DropPrimaryKey(
                name: "PK_humidity_reading",
                table: "humidity_reading");

            migrationBuilder.DropPrimaryKey(
                name: "PK_home",
                table: "home");

            migrationBuilder.DropPrimaryKey(
                name: "PK_door",
                table: "door");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admin",
                table: "admin");

            migrationBuilder.RenameTable(
                name: "temperature_reading",
                newName: "TemperatureReadings");

            migrationBuilder.RenameTable(
                name: "room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "member",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "light_reading",
                newName: "LightReadings");

            migrationBuilder.RenameTable(
                name: "humidity_reading",
                newName: "HumidityReadings");

            migrationBuilder.RenameTable(
                name: "home",
                newName: "Homes");

            migrationBuilder.RenameTable(
                name: "door",
                newName: "Doors");

            migrationBuilder.RenameTable(
                name: "admin",
                newName: "Admins");

            migrationBuilder.RenameIndex(
                name: "IX_temperature_reading_RoomId",
                table: "TemperatureReadings",
                newName: "IX_TemperatureReadings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_room_HomeId",
                table: "Rooms",
                newName: "IX_Rooms_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_notification_HomeId",
                table: "Notifications",
                newName: "IX_Notifications_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_member_HomeId",
                table: "Members",
                newName: "IX_Members_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_member_AdminId",
                table: "Members",
                newName: "IX_Members_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_light_reading_RoomId",
                table: "LightReadings",
                newName: "IX_LightReadings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_humidity_reading_RoomId",
                table: "HumidityReadings",
                newName: "IX_HumidityReadings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_door_HomeId",
                table: "Doors",
                newName: "IX_Doors_HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_admin_HomeId",
                table: "Admins",
                newName: "IX_Admins_HomeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemperatureReadings",
                table: "TemperatureReadings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LightReadings",
                table: "LightReadings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HumidityReadings",
                table: "HumidityReadings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homes",
                table: "Homes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doors",
                table: "Doors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Homes_HomeId",
                table: "Admins",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Homes_HomeId",
                table: "Doors",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HumidityReadings_Rooms_RoomId",
                table: "HumidityReadings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LightReadings_Rooms_RoomId",
                table: "LightReadings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Admins_AdminId",
                table: "Members",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Homes_HomeId",
                table: "Members",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Homes_HomeId",
                table: "Notifications",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Homes_HomeId",
                table: "Rooms",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemperatureReadings_Rooms_RoomId",
                table: "TemperatureReadings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
