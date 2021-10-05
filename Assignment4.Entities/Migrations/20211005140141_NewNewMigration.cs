using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment4.Entities.Migrations
{
    public partial class NewNewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tags_TagsID",
                table: "TagTask");

            migrationBuilder.RenameColumn(
                name: "TagsID",
                table: "TagTask",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tags",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tags_TagsId",
                table: "TagTask",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tags_TagsId",
                table: "TagTask");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "TagTask",
                newName: "TagsID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tags",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tags_TagsID",
                table: "TagTask",
                column: "TagsID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
