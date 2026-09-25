using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SANDBOX.Migrations
{
    /// <inheritdoc />
    public partial class AddHomework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeworkProblem",
                columns: table => new
                {
                    HWsId = table.Column<int>(type: "int", nullable: false),
                    ProblemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkProblem", x => new { x.HWsId, x.ProblemsId });
                    table.ForeignKey(
                        name: "FK_HomeworkProblem_Homeworks_HWsId",
                        column: x => x.HWsId,
                        principalTable: "Homeworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeworkProblem_Problems_ProblemsId",
                        column: x => x.ProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkProblem_ProblemsId",
                table: "HomeworkProblem",
                column: "ProblemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_GroupId",
                table: "Homeworks",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkProblem");

            migrationBuilder.DropTable(
                name: "Homeworks");
        }
    }
}
