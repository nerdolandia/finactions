using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    DataModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW()"),
                    Nome = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
