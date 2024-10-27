using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGM.Database.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCoreTablesForAGM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentEventTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEventTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscordServerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelEventDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    EventMessageDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ContentEventSubTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEventTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEventSubTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ContentEventSubTypes_ContentEventTypes_ContentEventTypeId",
                        column: x => x.ContentEventTypeId,
                        principalTable: "ContentEventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartsOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    ModifiedByByDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    AlbionMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEvents", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ContentEvents_ContentEventSubTypes_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "ContentEventSubTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentEvents_ContentEventTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ContentEventTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentEvents_Maps_AlbionMapId",
                        column: x => x.AlbionMapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentEvents_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentEvents_AlbionMapId",
                table: "ContentEvents",
                column: "AlbionMapId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentEvents_SubTypeId",
                table: "ContentEvents",
                column: "SubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentEvents_TenantId",
                table: "ContentEvents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentEvents_TypeId",
                table: "ContentEvents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentEventSubTypes_ContentEventTypeId",
                table: "ContentEventSubTypes",
                column: "ContentEventTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentEvents");

            migrationBuilder.DropTable(
                name: "ContentEventSubTypes");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "ContentEventTypes");
        }
    }
}
