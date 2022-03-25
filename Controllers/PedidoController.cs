using Lanche.Models;
using Lanche.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lanche.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Checkout(TbPedido tbPedido)
        {

            int totalItensNoPedido = 0;
            decimal precoTotalDoPedido = 0.0m;

            //obtem os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> itens = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.CarrinhoCompraItens = itens;
            //verifica se existem itens de pedido
            if (_carrinhoCompra.CarrinhoCompraItens.Count == 0) //Verificando se o carrinho tem intes, se não tiver vai mostrar um msg 
            {
                ModelState.AddModelError("", "Seu carrinho esta vazio, que tal incluir um lanche...");
            }

            //calcula o total de itens e o total do pedido
            foreach (var item in itens)
            {
                totalItensNoPedido += item.Quantidade;
                precoTotalDoPedido += (item.Produto.Preco * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido

            tbPedido.TotalItensPedido = totalItensNoPedido;
            tbPedido.PedidoTotal = precoTotalDoPedido;

            //valida os dados do pedido
            if (ModelState.IsValid)
            {
                //cria o pedido e os detalhes
                _pedidoRepository.CriarPedido(tbPedido);

                //define mensagens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :)";
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                //limpa o carrinho do cliente
                _carrinhoCompra.LimparCarrinho();

                //exibe a view com dados do cliente e do pedido

                return View("~/Views/Pedido/CheckoutCompleto.cshtml", tbPedido);
            }

            return View();
        }

    }
}
