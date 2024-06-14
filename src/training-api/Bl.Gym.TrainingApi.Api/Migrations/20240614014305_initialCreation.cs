using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bl.Gym.TrainingApi.Api.Migrations
{
    /// <inheritdoc />
    public partial class initialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GymGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LastName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UserName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PasswordSalt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SecurityStamp = table.Column<Guid>(type: "uuid", nullable: true),
                    ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ClaimValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainingRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GymGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainingRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTrainingRoles_GymGroups_GymGroupId",
                        column: x => x.GymGroupId,
                        principalTable: "GymGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainingRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainingRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainingSheets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GymId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainingSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTrainingSheets_GymGroups_GymId",
                        column: x => x.GymId,
                        principalTable: "GymGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainingSheets_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTrainingSheetId = table.Column<Guid>(type: "uuid", nullable: false),
                    MuscularGroup = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    TargetDaysCount = table.Column<int>(type: "integer", nullable: false),
                    CurrentDaysCount = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSections_UserTrainingSheets_UserTrainingSheetId",
                        column: x => x.UserTrainingSheetId,
                        principalTable: "UserTrainingSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainingSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Set = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseSets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseSets_TrainingSections_TrainingSectionId",
                        column: x => x.TrainingSectionId,
                        principalTable: "TrainingSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSets_ExerciseId",
                table: "ExerciseSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSets_TrainingSectionId",
                table: "ExerciseSets",
                column: "TrainingSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NormalizedName",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSections_UserTrainingSheetId",
                table: "TrainingSections",
                column: "UserTrainingSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUserName",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingRoles_GymGroupId",
                table: "UserTrainingRoles",
                column: "GymGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingRoles_RoleId",
                table: "UserTrainingRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingRoles_UserId",
                table: "UserTrainingRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingSheets_GymId",
                table: "UserTrainingSheets",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingSheets_StudentId",
                table: "UserTrainingSheets",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseSets");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserTrainingRoles");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "TrainingSections");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserTrainingSheets");

            migrationBuilder.DropTable(
                name: "GymGroups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
