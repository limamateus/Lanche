using Lanche.Models;
using Lanche.Repositories.Interfaces;
using Lanche.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lanche.Controllers
{
    public class ProdutosController : Controller
    {

        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }


        public IActionResult List(string categoria)
        {

            //var produtos = _produtoRepository.Produtos;
            //return View(produtos);

            //var produtoViewModel = new ProdutoListViewModel();
            //produtoViewModel.Produtos = _produtoRepository.Produtos;
            //produtoViewModel.CategoriaAtual = "Categoria atual";

            IEnumerable<Produto> produtos;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(categoria))
            {
                produtos = _produtoRepository.Produtos.OrderBy(p => p.ProdutoId);
                categoriaAtual = "Todos os produtos";
            }
            else
            {
                produtos = _produtoRepository.Produtos.Where(p => p.Categoria.CategoriaNome
                .Equals(categoria)).OrderBy(c => c.Nome);
                categoriaAtual = categoria;
            }

            var produtosListViewModel = new ProdutoListViewModel
            {
                Produtos = produtos,
                CategoriaAtual = categoria
            };

            return View(produtosListViewModel);

        }

        public IActionResult Details(int produtoId)
        {
            var produto = _produtoRepository.Produtos.FirstOrDefault(p => p.ProdutoId == produtoId);
            return View(produto);
        }

        public ViewResult Search(string searchString)
        {

            IEnumerable<Produto> produtos; // um lista de produtos
            string categoriaAtual = string.Empty;


            if (string.IsNullOrEmpty(searchString)) // Verificando se o campo de pesquisa é nulo, se for ele vai retornar todos os lanches
            {
                produtos = _produtoRepository.Produtos.OrderBy(p => p.ProdutoId); // ordenar todos os lanches por ID
                categoriaAtual = "Todos os Lanches";
            }
            else
            { // vai procuro o produto de acorco com o que foi digitado 
                produtos = _produtoRepository.Produtos
                    .Where(p => p.Nome.ToLower().Contains(searchString.ToLower()));

                if (produtos.Any())
                {
                    categoriaAtual = "Lanche";
                }
                else
                {
                    categoriaAtual = "Produto não existe";
                }

               
            }

            return View("~/Views/Produtos/List.cshtml", new ProdutoListViewModel
            {

                Produtos = produtos,
                CategoriaAtual = categoriaAtual



            });

        }
    }
}
