using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workbalance.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WB_USER",
                columns: table => new
                {
                    CD_USER_ID = table.Column<string>(type: "VARCHAR2(36)", nullable: false),
                    NM_NAME = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    DS_PASSWORD_HASH = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DS_PREFERRED_LANGUAGE = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false, defaultValueSql: "'pt-BR'"),
                    TS_CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    TS_UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WB_USER", x => x.CD_USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "WB_MOOD_ENTRY",
                columns: table => new
                {
                    CD_MOOD_ID = table.Column<string>(type: "VARCHAR2(36)", nullable: false),
                    CD_USER_ID = table.Column<string>(type: "VARCHAR2(36)", nullable: false),
                    DT_DATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    NR_MOOD = table.Column<byte>(type: "NUMBER(3)", nullable: false),
                    NR_STRESS = table.Column<byte>(type: "NUMBER(3)", nullable: false),
                    NR_PRODUCTIVITY = table.Column<byte>(type: "NUMBER(3)", nullable: false),
                    DS_NOTES = table.Column<string>(type: "VARCHAR2(500)", nullable: true),
                    DS_TAGS = table.Column<string>(type: "VARCHAR2(500)", nullable: true),
                    TS_CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(0)", nullable: false),
                    TS_UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WB_MOOD_ENTRY", x => x.CD_MOOD_ID);
                    table.ForeignKey(
                        name: "FK_WB_MOOD_ENTRY_WB_USER_CD_USER_ID",
                        column: x => x.CD_USER_ID,
                        principalTable: "WB_USER",
                        principalColumn: "CD_USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WB_RECOMMENDATION",
                columns: table => new
                {
                    CD_RECOMMENDATION_ID = table.Column<string>(type: "VARCHAR2(36)", nullable: false),
                    CD_USER_ID = table.Column<string>(type: "VARCHAR2(36)", nullable: false),
                    DS_TYPE = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    DS_MESSAGE = table.Column<string>(type: "VARCHAR2(200)", nullable: false),
                    DS_ACTION_URL = table.Column<string>(type: "VARCHAR2(300)", nullable: true),
                    TS_SCHEDULED_AT = table.Column<DateTime>(type: "TIMESTAMP(0)", nullable: true),
                    DS_SOURCE = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    TS_CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WB_RECOMMENDATION", x => x.CD_RECOMMENDATION_ID);
                    table.ForeignKey(
                        name: "FK_WB_RECOMMENDATION_WB_USER_CD_USER_ID",
                        column: x => x.CD_USER_ID,
                        principalTable: "WB_USER",
                        principalColumn: "CD_USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_WB_MOOD_USER_DATE",
                table: "WB_MOOD_ENTRY",
                columns: new[] { "CD_USER_ID", "DT_DATE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_WB_RECOM_USER_CREATED",
                table: "WB_RECOMMENDATION",
                columns: new[] { "CD_USER_ID", "TS_CREATED_AT" });

            migrationBuilder.CreateIndex(
                name: "IX_WB_USER_DS_EMAIL",
                table: "WB_USER",
                column: "DS_EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WB_MOOD_ENTRY");

            migrationBuilder.DropTable(
                name: "WB_RECOMMENDATION");

            migrationBuilder.DropTable(
                name: "WB_USER");
        }
    }
}
