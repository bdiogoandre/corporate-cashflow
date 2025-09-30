using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.Cashflow.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class LastTransactionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastTransactionId",
                table: "AccountBalances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTransactionId",
                table: "AccountBalances");
        }
    }
}
