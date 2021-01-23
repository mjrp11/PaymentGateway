using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Migrations.Migrations
{
    public partial class AddDateCreatedAndModifiedToPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(@" CREATE TRIGGER PaymentDateModified 
                                    ON Payments
                                    AFTER UPDATE
                                    AS BEGIN
                                    SET NOCOUNT ON;
                                    UPDATE Payments SET DateModified=GETUTCDATE() 
                                    FROM INSERTED i
                                    WHERE i.PaymentId = Payments.PaymentId 
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS PaymentDateModified");
            
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Payments");
        }
    }
}
