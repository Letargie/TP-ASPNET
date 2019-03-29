using Microsoft.EntityFrameworkCore.Migrations;

namespace TPASPNET.Migrations
{
    public partial class todolabelbos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLabel_Label_LabelGuid",
                table: "TodoLabel");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoLabel_Todo_TodoGuid",
                table: "TodoLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoLabel",
                table: "TodoLabel");

            migrationBuilder.RenameTable(
                name: "TodoLabel",
                newName: "TodoLabels");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLabel_TodoGuid",
                table: "TodoLabels",
                newName: "IX_TodoLabels_TodoGuid");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLabel_LabelGuid",
                table: "TodoLabels",
                newName: "IX_TodoLabels_LabelGuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoLabels",
                table: "TodoLabels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLabels_Label_LabelGuid",
                table: "TodoLabels",
                column: "LabelGuid",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLabels_Todo_TodoGuid",
                table: "TodoLabels",
                column: "TodoGuid",
                principalTable: "Todo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLabels_Label_LabelGuid",
                table: "TodoLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoLabels_Todo_TodoGuid",
                table: "TodoLabels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoLabels",
                table: "TodoLabels");

            migrationBuilder.RenameTable(
                name: "TodoLabels",
                newName: "TodoLabel");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLabels_TodoGuid",
                table: "TodoLabel",
                newName: "IX_TodoLabel_TodoGuid");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLabels_LabelGuid",
                table: "TodoLabel",
                newName: "IX_TodoLabel_LabelGuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoLabel",
                table: "TodoLabel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLabel_Label_LabelGuid",
                table: "TodoLabel",
                column: "LabelGuid",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLabel_Todo_TodoGuid",
                table: "TodoLabel",
                column: "TodoGuid",
                principalTable: "Todo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
