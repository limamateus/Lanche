using Lanche.Models;

namespace Lanche.ViewModels
{
    public class PedidoProdutoViewModels
    {

        public TbPedido  TbPedido { get; set; }

        public IEnumerable<TbPedidoDetalhe> TbPedidoDetalhe { get; set; }


    }
}
