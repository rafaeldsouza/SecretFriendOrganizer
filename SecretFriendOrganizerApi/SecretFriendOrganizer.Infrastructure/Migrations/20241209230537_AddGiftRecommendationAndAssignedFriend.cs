using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretFriendOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGiftRecommendationAndAssignedFriend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GiftRecommendation",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberships_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                column: "AssignedFriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberships_Users_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships",
                column: "AssignedFriendId",
                principalSchema: "secret_friend_db",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberships_Users_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships");

            migrationBuilder.DropIndex(
                name: "IX_GroupMemberships_AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships");

            migrationBuilder.DropColumn(
                name: "AssignedFriendId",
                schema: "secret_friend_db",
                table: "GroupMemberships");

            migrationBuilder.DropColumn(
                name: "GiftRecommendation",
                schema: "secret_friend_db",
                table: "GroupMemberships");
        }
    }
}
