using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TPASPNET.Migrations
{
    public partial class TodoLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoLabel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TodoGuid = table.Column<Guid>(nullable: false),
                    LabelGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoLabel_Label_LabelGuid",
                        column: x => x.LabelGuid,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoLabel_Todo_TodoGuid",
                        column: x => x.TodoGuid,
                        principalTable: "Todo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoLabel_LabelGuid",
                table: "TodoLabel",
                column: "LabelGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLabel_TodoGuid",
                table: "TodoLabel",
                column: "TodoGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoLabel");
        }
    }
}
