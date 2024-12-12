using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretFriendOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addkeycloackId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberships_Users_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships");

            migrationBuilder.AddColumn<string>(
                name: "KeycloakId",
                schema: "secret_friend_db",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "GiftRecommendation",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberships_Users_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                column: "AssignedFriendId",
                principalSchema: "secret_friend_db",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberships_Users_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships");

            migrationBuilder.DropColumn(
                name: "KeycloakId",
                schema: "secret_friend_db",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "GiftRecommendation",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberships_Users_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                column: "AssignedFriendId",
                principalSchema: "secret_friend_db",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
