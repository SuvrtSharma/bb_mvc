﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionFieldtoProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedBy",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedOn",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Status", "SubmittedBy", "SubmittedOn" },
                values: new object[] { 1, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Status", "SubmittedBy", "SubmittedOn" },
                values: new object[] { 1, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Status", "SubmittedBy", "SubmittedOn" },
                values: new object[] { 1, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Status", "SubmittedBy", "SubmittedOn" },
                values: new object[] { 1, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Status", "SubmittedBy", "SubmittedOn" },
                values: new object[] { 1, null, null });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Status", "SubmittedBy", "SubmittedOn" },
                values: new object[] { 1, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SubmittedOn",
                table: "Product");
        }
    }
}
