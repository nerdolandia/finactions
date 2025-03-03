using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovimentacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "TipoConta",
                table: "ContasBancarias",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Movimentacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW()"),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    TipoMovimentacao = table.Column<byte>(type: "smallint", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: false),
                    Tag = table.Column<string>(type: "text", unicode: false, nullable: true),
                    Cor = table.Column<string>(type: "text", unicode: false, nullable: true),
                    ValorMovimentado = table.Column<decimal>(type: "money", nullable: false),
                    DataMovimentacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ContaBancariaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_ContasBancarias_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_CategoriaId",
                table: "Movimentacoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_ContaBancariaId",
                table: "Movimentacoes",
                column: "ContaBancariaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimentacoes");

            migrationBuilder.AlterColumn<int>(
                name: "TipoConta",
                table: "ContasBancarias",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
