using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSG.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserAuthTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAuth",
                columns: table => new
                {
                    UserAuthId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    ElementId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsVisible = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedById = table.Column<long>(type: "INTEGER", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedById = table.Column<long>(type: "INTEGER", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuth", x => x.UserAuthId);
                    table.ForeignKey(
                        name: "FK_UserAuth_Element_ElementId",
                        column: x => x.ElementId,
                        principalTable: "Element",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAuth_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAuth_ElementId",
                table: "UserAuth",
                column: "ElementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuth_UserId_ElementId",
                table: "UserAuth",
                columns: new[] { "UserId", "ElementId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuth");
        }
    }
}
