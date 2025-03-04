using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Movimentacoes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Movimentacoes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContasBancarias",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ContasBancarias",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categorias",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Categorias",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_UserId_Id",
                table: "Movimentacoes",
                columns: new[] { "UserId", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContasBancarias_UserId_Id",
                table: "ContasBancarias",
                columns: new[] { "UserId", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UserId_Id",
                table: "Categorias",
                columns: new[] { "UserId", "Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Users_UserId",
                table: "Categorias",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContasBancarias_Users_UserId",
                table: "ContasBancarias",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_Users_UserId",
                table: "Movimentacoes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Users_UserId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ContasBancarias_Users_UserId",
                table: "ContasBancarias");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_Users_UserId",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_UserId_Id",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_ContasBancarias_UserId_Id",
                table: "ContasBancarias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UserId_Id",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Movimentacoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movimentacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContasBancarias");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContasBancarias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categorias");
        }
    }
}
