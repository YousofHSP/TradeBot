using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateDateAndProjectKind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDate",
                schema: "rad",
                table: "ApiTokens");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "UserGroups",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "UploadedFiles",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "SubSystems",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "SubSystemConfigurations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "PhoneNumberHistories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "PasswordHistories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "Notifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "IpRules",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "IpAccessTypes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "ImportedRecords",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "ImportedFiles",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "EmailHistories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "Backups",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "AuditChecks",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateDate",
                schema: "rad",
                table: "ArchiveLogs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "ProjectKinds",
                schema: "rad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectKinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectKinds_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalSchema: "rad",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectKinds_CreatorUserId",
                schema: "rad",
                table: "ProjectKinds",
                column: "CreatorUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectKinds",
                schema: "rad");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "UploadedFiles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "SubSystems");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "SubSystemConfigurations");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "PhoneNumberHistories");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "PasswordHistories");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "IpRules");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "IpAccessTypes");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "ImportedRecords");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "ImportedFiles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "EmailHistories");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "Backups");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "AuditChecks");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "rad",
                table: "ArchiveLogs");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeleteDate",
                schema: "rad",
                table: "ApiTokens",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
