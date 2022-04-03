using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalRStreaming.BL.Migrations
{
    public partial class addedMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    ChatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatFriendsID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.ChatID);
                    table.ForeignKey(
                        name: "FK_ChatMessage_ChatFriends_ChatFriendsID",
                        column: x => x.ChatFriendsID,
                        principalTable: "ChatFriends",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChatFriendsID",
                table: "ChatMessage",
                column: "ChatFriendsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessage");
        }
    }
}
