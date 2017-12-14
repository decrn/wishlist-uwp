using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ServerApp.Migrations
{
    public partial class RenameUserListSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserListSubscriptions");

            migrationBuilder.CreateTable(
                name: "UserListSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserListSubscription_List_ListId",
                        column: x => x.ListId,
                        principalTable: "List",
                        principalColumn: "ListId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListSubscription_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserListSubscription_ListId",
                table: "UserListSubscription",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListSubscription_UserId",
                table: "UserListSubscription",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserListSubscription");

            migrationBuilder.CreateTable(
                name: "UserListSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ListId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserListSubscriptions_List_ListId",
                        column: x => x.ListId,
                        principalTable: "List",
                        principalColumn: "ListId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListSubscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserListSubscriptions_ListId",
                table: "UserListSubscriptions",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListSubscriptions_UserId",
                table: "UserListSubscriptions",
                column: "UserId");
        }
    }
}
