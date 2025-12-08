using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignee",
                table: "TaskAssignee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectUser",
                table: "ProjectUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannerTask",
                table: "PlannerTask");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "TaskAssignee",
                newName: "taskAssignees");

            migrationBuilder.RenameTable(
                name: "ProjectUser",
                newName: "projectUsers");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "projects");

            migrationBuilder.RenameTable(
                name: "PlannerTask",
                newName: "tasks");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_taskAssignees",
                table: "taskAssignees",
                columns: new[] { "TaskId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_projectUsers",
                table: "projectUsers",
                columns: new[] { "UserId", "ProjectId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_projects",
                table: "projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tasks",
                table: "tasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_taskAssignees_UserId",
                table: "taskAssignees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_projectUsers_ProjectId",
                table: "projectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_projectUsers_UserId",
                table: "projectUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ProjectId",
                table: "tasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_projectUsers_projects_ProjectId",
                table: "projectUsers",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_projectUsers_users_UserId",
                table: "projectUsers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_taskAssignees_tasks_TaskId",
                table: "taskAssignees",
                column: "TaskId",
                principalTable: "tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_taskAssignees_users_UserId",
                table: "taskAssignees",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_projects_ProjectId",
                table: "tasks",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projectUsers_projects_ProjectId",
                table: "projectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_projectUsers_users_UserId",
                table: "projectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_taskAssignees_tasks_TaskId",
                table: "taskAssignees");

            migrationBuilder.DropForeignKey(
                name: "FK_taskAssignees_users_UserId",
                table: "taskAssignees");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_projects_ProjectId",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tasks",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_ProjectId",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_taskAssignees",
                table: "taskAssignees");

            migrationBuilder.DropIndex(
                name: "IX_taskAssignees_UserId",
                table: "taskAssignees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_projectUsers",
                table: "projectUsers");

            migrationBuilder.DropIndex(
                name: "IX_projectUsers_ProjectId",
                table: "projectUsers");

            migrationBuilder.DropIndex(
                name: "IX_projectUsers_UserId",
                table: "projectUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_projects",
                table: "projects");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "tasks",
                newName: "PlannerTask");

            migrationBuilder.RenameTable(
                name: "taskAssignees",
                newName: "TaskAssignee");

            migrationBuilder.RenameTable(
                name: "projectUsers",
                newName: "ProjectUser");

            migrationBuilder.RenameTable(
                name: "projects",
                newName: "Project");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "User",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannerTask",
                table: "PlannerTask",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignee",
                table: "TaskAssignee",
                columns: new[] { "TaskId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectUser",
                table: "ProjectUser",
                columns: new[] { "ProjectId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");
        }
    }
}
