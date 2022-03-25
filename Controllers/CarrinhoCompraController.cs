using Lanche.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Lanche.Models;
using Lanche.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Lanche.Controllers
{
    public class CarrinhoCompraController : Controller
    { 
        private readonly IProdutoRepository _produtoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;
public CarrinhoCompraController(IProdutoRepository produtoRepository, CarrinhoCompra carrinhoCompra)
        {
            _produtoRepository = produtoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Index()
        {
            // essa veriavel vai conter os itens do carrinho existente
            var itens = _carrinhoCompra.GetCarrinhoCompraItems();
            // Depois vai pegar os itens e jogar na propriendade CarrinhCompraItens
            _carrinhoCompra.CarrinhoCompraItens = itens;
            // Alinha a baixa eu estou criando um instancia onde vou pegar os resutados acima
            // e jogar nas propriedades da minha ViewModel
            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };
            // aqui estou retornando o valor que colocado no carrinhoCompraVM
                return View(carrinhoCompraVM);
        }

        [Authorize]
        public RedirectToActionResult AdicionarItemNoCarrinhoCompra(int produtoId)
        {  // ele vai obter o lanche dentro do repositorio 
            var produtoSelecionado = _produtoRepository.Produtos.FirstOrDefault(p=> p.ProdutoId == produtoId);
            if (produtoSelecionado != null)
            {
                _carrinhoCompra.AdicionarAoCarrinho(produtoSelecionado);
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult RemoverItemDoCarrinhoCompra(int produtoId)
        {
            var produtoSelecionado = _produtoRepository.Produtos.FirstOrDefault
                                    (p => p.ProdutoId == produtoId);
            if(produtoSelecionado != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(produtoSelecionado);
            }

            return RedirectToAction("Index");
        }

    }
}
