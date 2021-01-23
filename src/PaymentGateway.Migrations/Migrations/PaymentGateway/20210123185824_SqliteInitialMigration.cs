using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Migrations.Migrations.PaymentGateway
{
    public partial class SqliteInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CardNumberMask = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ExpireDateYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpireDateMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    CVV = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 20, scale: 3, nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    BankPaymentIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardNumber_CVV_ExpireDateMonth_ExpireDateYear",
                table: "Cards",
                columns: new[] { "CardNumber", "CVV", "ExpireDateMonth", "ExpireDateYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardId",
                table: "Payments",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UniqueId",
                table: "Payments",
                column: "UniqueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
