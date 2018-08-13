using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityClaims.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lookup",
                columns: table => new
                {
                    LookupID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActiveFlag = table.Column<string>(maxLength: 1, nullable: false),
                    Attribute1 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute2 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute3 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute4 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute5 = table.Column<string>(maxLength: 128, nullable: true),
                    LookupCode = table.Column<string>(maxLength: 48, nullable: false),
                    LookupDesc = table.Column<string>(maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup", x => x.LookupID);
                });

            migrationBuilder.CreateTable(
                name: "LookupValue",
                columns: table => new
                {
                    LookupValueID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActiveFlag = table.Column<string>(maxLength: 1, nullable: false),
                    Attribute1 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute2 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute3 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute4 = table.Column<string>(maxLength: 128, nullable: true),
                    Attribute5 = table.Column<string>(maxLength: 128, nullable: true),
                    LookupID = table.Column<int>(nullable: false),
                    LookupValueCode = table.Column<string>(maxLength: 48, nullable: false),
                    LookupValueDesc = table.Column<string>(maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupValue", x => x.LookupValueID);
                    table.ForeignKey(
                        name: "FK_LookupValue_Lookup_LookupID",
                        column: x => x.LookupID,
                        principalTable: "Lookup",
                        principalColumn: "LookupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LookupValue_LookupID",
                table: "LookupValue",
                column: "LookupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LookupValue");

            migrationBuilder.DropTable(
                name: "Lookup");
        }
    }
}
