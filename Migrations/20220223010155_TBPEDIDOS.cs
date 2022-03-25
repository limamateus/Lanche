using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lanche.Migrations
{
    public partial class TBPEDIDOS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbPedidos",
                columns: table => new
                {
                    TbPedidoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Endereco1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Endereco2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PedidoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItensPedido = table.Column<int>(type: "int", nullable: false),
                    PedidoEnviado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PedidoEntregueEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPedidos", x => x.TbPedidoId);
                });

            migrationBuilder.CreateTable(
                name: "TbPedidoDetalhes",
                columns: table => new
                {
                    TbPedidoDetalheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TbPedidoId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPedidoDetalhes", x => x.TbPedidoDetalheId);
                    table.ForeignKey(
                        name: "FK_TbPedidoDetalhes_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbPedidoDetalhes_TbPedidos_TbPedidoId",
                        column: x => x.TbPedidoId,
                        principalTable: "TbPedidos",
                        principalColumn: "TbPedidoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbPedidoDetalhes_ProdutoId",
                table: "TbPedidoDetalhes",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPedidoDetalhes_TbPedidoId",
                table: "TbPedidoDetalhes",
                column: "TbPedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbPedidoDetalhes");

            migrationBuilder.DropTable(
                name: "TbPedidos");
        }
    }
}
