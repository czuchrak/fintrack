using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrack.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "dbo",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                schema: "dbo",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(8,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => new { x.Date, x.Currency });
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsCost = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                schema: "dbo",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewMonthEmailEnabled = table.Column<bool>(type: "bit", nullable: false),
                    NewsEmailEnabled = table.Column<bool>(type: "bit", nullable: false),
                    VerificationMailSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NetWorthEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetWorthEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetWorthEntries_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetWorthGoals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReturnRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetWorthGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetWorthGoals_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetWorthParts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetWorthParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetWorthParts_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                schema: "dbo",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => new { x.NotificationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "dbo",
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetWorthEntryParts",
                schema: "dbo",
                columns: table => new
                {
                    NetWorthEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NetWorthPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetWorthEntryParts", x => new { x.NetWorthPartId, x.NetWorthEntryId });
                    table.ForeignKey(
                        name: "FK_NetWorthEntryParts_NetWorthEntries_NetWorthEntryId",
                        column: x => x.NetWorthEntryId,
                        principalSchema: "dbo",
                        principalTable: "NetWorthEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetWorthEntryParts_NetWorthParts_NetWorthPartId",
                        column: x => x.NetWorthPartId,
                        principalSchema: "dbo",
                        principalTable: "NetWorthParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetWorthGoalParts",
                schema: "dbo",
                columns: table => new
                {
                    NetWorthGoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NetWorthPartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetWorthGoalParts", x => new { x.NetWorthPartId, x.NetWorthGoalId });
                    table.ForeignKey(
                        name: "FK_NetWorthGoalParts_NetWorthGoals_NetWorthGoalId",
                        column: x => x.NetWorthGoalId,
                        principalSchema: "dbo",
                        principalTable: "NetWorthGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetWorthGoalParts_NetWorthParts_NetWorthPartId",
                        column: x => x.NetWorthPartId,
                        principalSchema: "dbo",
                        principalTable: "NetWorthParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTransactions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyTransactions_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "dbo",
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NetWorthEntries_UserId",
                schema: "dbo",
                table: "NetWorthEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NetWorthEntryParts_NetWorthEntryId",
                schema: "dbo",
                table: "NetWorthEntryParts",
                column: "NetWorthEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_NetWorthGoalParts_NetWorthGoalId",
                schema: "dbo",
                table: "NetWorthGoalParts",
                column: "NetWorthGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_NetWorthGoals_UserId",
                schema: "dbo",
                table: "NetWorthGoals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NetWorthParts_UserId",
                schema: "dbo",
                table: "NetWorthParts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UserId",
                schema: "dbo",
                table: "Properties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTransactions_PropertyId",
                schema: "dbo",
                table: "PropertyTransactions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                schema: "dbo",
                table: "UserNotifications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExchangeRates",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Logs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NetWorthEntryParts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NetWorthGoalParts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PropertyCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PropertyTransactions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Settings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserNotifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NetWorthEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NetWorthGoals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NetWorthParts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Properties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");
        }
    }
}
