using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SubSystems_SubSystemId",
                schema: "rad",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubSystemId",
                schema: "rad",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubSystemId",
                schema: "rad",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserGroups",
                schema: "rad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubSystemId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_SubSystems_SubSystemId",
                        column: x => x.SubSystemId,
                        principalSchema: "rad",
                        principalTable: "SubSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalSchema: "rad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleUserGroups",
                schema: "rad",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UserGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUserGroups", x => new { x.RolesId, x.UserGroupsId });
                    table.ForeignKey(
                        name: "FK_RoleUserGroups_Roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "rad",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleUserGroups_UserGroups_UserGroupsId",
                        column: x => x.UserGroupsId,
                        principalSchema: "rad",
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserUserGroups",
                schema: "rad",
                columns: table => new
                {
                    UserGroupsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserGroups", x => new { x.UserGroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserUserGroups_UserGroups_UserGroupsId",
                        column: x => x.UserGroupsId,
                        principalSchema: "rad",
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserUserGroups_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "rad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUserGroups_UserGroupsId",
                schema: "rad",
                table: "RoleUserGroups",
                column: "UserGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_CreatorUserId",
                schema: "rad",
                table: "UserGroups",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_SubSystemId",
                schema: "rad",
                table: "UserGroups",
                column: "SubSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroups_UsersId",
                schema: "rad",
                table: "UserUserGroups",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUserGroups",
                schema: "rad");

            migrationBuilder.DropTable(
                name: "UserUserGroups",
                schema: "rad");

            migrationBuilder.DropTable(
                name: "UserGroups",
                schema: "rad");

            migrationBuilder.AddColumn<int>(
                name: "SubSystemId",
                schema: "rad",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubSystemId",
                schema: "rad",
                table: "Users",
                column: "SubSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SubSystems_SubSystemId",
                schema: "rad",
                table: "Users",
                column: "SubSystemId",
                principalSchema: "rad",
                principalTable: "SubSystems",
                principalColumn: "Id");
        }
    }
}
