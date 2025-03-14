using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TabelasChavesCompostasCorretas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_Categorias_CategoriaId",
                table: "Movimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_ContasBancarias_ContaBancariaId",
                table: "Movimentacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movimentacoes",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_CategoriaId",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_ContaBancariaId",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_UserId_Id",
                table: "Movimentacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContasBancarias",
                table: "ContasBancarias");

            migrationBuilder.DropIndex(
                name: "IX_ContasBancarias_UserId_Id",
                table: "ContasBancarias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UserId_Id",
                table: "Categorias");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movimentacoes",
                table: "Movimentacoes",
                columns: new[] { "UserId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContasBancarias",
                table: "ContasBancarias",
                columns: new[] { "UserId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                columns: new[] { "UserId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_CategoriaId_UserId",
                table: "Movimentacoes",
                columns: new[] { "CategoriaId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_ContaBancariaId_UserId",
                table: "Movimentacoes",
                columns: new[] { "ContaBancariaId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_Categorias_CategoriaId_UserId",
                table: "Movimentacoes",
                columns: new[] { "CategoriaId", "UserId" },
                principalTable: "Categorias",
                principalColumns: new[] { "UserId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_ContasBancarias_ContaBancariaId_UserId",
                table: "Movimentacoes",
                columns: new[] { "ContaBancariaId", "UserId" },
                principalTable: "ContasBancarias",
                principalColumns: new[] { "UserId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_Categorias_CategoriaId_UserId",
                table: "Movimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_ContasBancarias_ContaBancariaId_UserId",
                table: "Movimentacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movimentacoes",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_CategoriaId_UserId",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_ContaBancariaId_UserId",
                table: "Movimentacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContasBancarias",
                table: "ContasBancarias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movimentacoes",
                table: "Movimentacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContasBancarias",
                table: "ContasBancarias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_CategoriaId",
                table: "Movimentacoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_ContaBancariaId",
                table: "Movimentacoes",
                column: "ContaBancariaId");

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
                name: "FK_Movimentacoes_Categorias_CategoriaId",
                table: "Movimentacoes",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_ContasBancarias_ContaBancariaId",
                table: "Movimentacoes",
                column: "ContaBancariaId",
                principalTable: "ContasBancarias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
