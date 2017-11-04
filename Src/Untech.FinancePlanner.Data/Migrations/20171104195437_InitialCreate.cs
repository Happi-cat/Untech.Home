using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Untech.FinancePlanner.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CacheEntries",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Json = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheEntries", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "FinancialJournalEntries",
                columns: table => new
                {
                    Key = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true),
                    TaxonKey = table.Column<int>(type: "INTEGER", nullable: false),
                    When = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualCurrency = table.Column<string>(type: "TEXT", nullable: true),
                    ForecastedCurrency = table.Column<string>(type: "TEXT", nullable: true),
                    ActualAmount = table.Column<double>(type: "REAL", nullable: false),
                    ForecastedAmount = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialJournalEntries", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Taxons",
                columns: table => new
                {
                    Key = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ParentKey = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxons", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CacheEntries");

            migrationBuilder.DropTable(
                name: "FinancialJournalEntries");

            migrationBuilder.DropTable(
                name: "Taxons");
        }
    }
}
