using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace transactions.portal.services.Data.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Reference = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Merchant = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PaymentBrand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CardLast4 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                CustomerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Disputes",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TransactionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CustomerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Disputes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Disputes_Transactions_TransactionId",
                    column: x => x.TransactionId,
                    principalTable: "Transactions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MerchantEntries",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TransactionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Reference = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                PostingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MerchantEntries", x => x.Id);
                table.ForeignKey(
                    name: "FK_MerchantEntries_Transactions_TransactionId",
                    column: x => x.TransactionId,
                    principalTable: "Transactions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Refunds",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Reference = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TransactionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Refunds", x => x.Id);
                table.ForeignKey(
                    name: "FK_Refunds_Transactions_TransactionId",
                    column: x => x.TransactionId,
                    principalTable: "Transactions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TransactionEvents",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TransactionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TransactionEvents", x => x.Id);
                table.ForeignKey(
                    name: "FK_TransactionEvents_Transactions_TransactionId",
                    column: x => x.TransactionId,
                    principalTable: "Transactions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DisputeStatusHistories",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                DisputeId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DisputeStatusHistories", x => x.Id);
                table.ForeignKey(
                    name: "FK_DisputeStatusHistories_DisputeId",
                    column: x => x.DisputeId,
                    principalTable: "Disputes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Disputes_CreatedAt",
            table: "Disputes",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Disputes_CustomerId",
            table: "Disputes",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Disputes_Status",
            table: "Disputes",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Disputes_TransactionId",
            table: "Disputes",
            column: "TransactionId");

        migrationBuilder.CreateIndex(
            name: "IX_DisputeStatusHistories_ChangedAt",
            table: "DisputeStatusHistories",
            column: "ChangedAt");

        migrationBuilder.CreateIndex(
            name: "IX_DisputeStatusHistories_DisputeId",
            table: "DisputeStatusHistories",
            column: "DisputeId");

        migrationBuilder.CreateIndex(
            name: "IX_MerchantEntries_TransactionId",
            table: "MerchantEntries",
            column: "TransactionId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Refunds_TransactionId",
            table: "Refunds",
            column: "TransactionId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_TransactionEvents_Timestamp",
            table: "TransactionEvents",
            column: "Timestamp");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionEvents_TransactionId",
            table: "TransactionEvents",
            column: "TransactionId");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_CustomerId",
            table: "Transactions",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_Date",
            table: "Transactions",
            column: "Date");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_Reference",
            table: "Transactions",
            column: "Reference",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_Status",
            table: "Transactions",
            column: "Status");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "DisputeStatusHistories");
        migrationBuilder.DropTable(name: "MerchantEntries");
        migrationBuilder.DropTable(name: "Refunds");
        migrationBuilder.DropTable(name: "TransactionEvents");
        migrationBuilder.DropTable(name: "Disputes");
        migrationBuilder.DropTable(name: "Transactions");
    }
}

