using Lanche.Context;
using Lanche.Models;
using Lanche.Repositories.Interfaces;

namespace Lanche.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoRepository(AppDbContext context, CarrinhoCompra carrinhoCompra)
        {
            _context = context;
            _carrinhoCompra = carrinhoCompra;
        }

        public void CriarPedido(TbPedido tbPedido)
        {
            tbPedido.PedidoEnviado = DateTime.Now; // defindo a data de recebimento do pedido 
            _context.TbPedidos.Add(tbPedido);
            _context.SaveChanges();


            var carrinhoCompraItens = _carrinhoCompra.CarrinhoCompraItens; // Receber os itens do carrinho

            foreach (var carrinhoItem in carrinhoCompraItens)
            {

                var pedidoDetail = new TbPedidoDetalhe()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    ProdutoId = carrinhoItem.Produto.ProdutoId,
                    TbPedidoId = tbPedido.TbPedidoId,
                    Preco = carrinhoItem.Produto.Preco

                };

                _context.TbPedidoDetalhes.Add(pedidoDetail);

            }

            _context.SaveChanges();

        }
    }
}
