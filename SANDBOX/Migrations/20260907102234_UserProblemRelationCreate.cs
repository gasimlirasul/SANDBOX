using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SANDBOX.Migrations
{
    /// <inheritdoc />
    public partial class UserProblemRelationCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProblemUser",
                columns: table => new
                {
                    SolvedProblemsId = table.Column<int>(type: "int", nullable: false),
                    UsersWhoSolvedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemUser", x => new { x.SolvedProblemsId, x.UsersWhoSolvedId });
                    table.ForeignKey(
                        name: "FK_ProblemUser_Problems_SolvedProblemsId",
                        column: x => x.SolvedProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemUser_Users_UsersWhoSolvedId",
                        column: x => x.UsersWhoSolvedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemUser_UsersWhoSolvedId",
                table: "ProblemUser",
                column: "UsersWhoSolvedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemUser");
        }
    }
}
