using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SANDBOX.Migrations
{
    /// <inheritdoc />
    public partial class addtagproblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProblemTag",
                columns: table => new
                {
                    ProblemsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTag", x => new { x.ProblemsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProblemTag_Problems_ProblemsId",
                        column: x => x.ProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTag_TagsId",
                table: "ProblemTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemTag");
        }
    }
}
