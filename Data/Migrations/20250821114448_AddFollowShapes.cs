using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowShapes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "rad",
                table: "ProjectKinds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "rad",
                table: "FormTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FollowShapeKinds",
                schema: "rad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeleteDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowShapeKinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowShapeKinds_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalSchema: "rad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FollowShapes",
                schema: "rad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FollowShapeKindId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeleteDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowShapes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowShapes_FollowShapeKinds_FollowShapeKindId",
                        column: x => x.FollowShapeKindId,
                        principalSchema: "rad",
                        principalTable: "FollowShapeKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowShapes_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalSchema: "rad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowShapeKinds_CreatorUserId",
                schema: "rad",
                table: "FollowShapeKinds",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowShapes_CreatorUserId",
                schema: "rad",
                table: "FollowShapes",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowShapes_FollowShapeKindId",
                schema: "rad",
                table: "FollowShapes",
                column: "FollowShapeKindId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowShapes",
                schema: "rad");

            migrationBuilder.DropTable(
                name: "FollowShapeKinds",
                schema: "rad");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "rad",
                table: "ProjectKinds");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "rad",
                table: "FormTypes");
        }
    }
}
