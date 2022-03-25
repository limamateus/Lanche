using Lanche.Context;
using Lanche.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Lanche.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]

    public class AdminProdutosController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AdminProdutosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //public IActionResult Index()
        //{
        //    return View(_appDbContext.Produtos.ToList()); ;
        //}

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        {
            var resultado = _appDbContext.Produtos.AsNoTracking().AsQueryable(); // consulta

            if (!string.IsNullOrWhiteSpace(filter)) // verificar se foi infomamdo 
            {
                resultado = resultado.Where(p => p.Nome.Contains(filter)); // e definir um outra consulta

            }
            // estou criando os dados paginados 
            var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");

            model.RouteValue = new RouteValueDictionary { { "filter", filter } }; // Rota que vai ser gerado e enviado para view


            return View(model);
        }
        public IActionResult Editar(int? id )
        {
            if(id == null)
            {
                return NotFound();
            }

            var produto = _appDbContext.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id ,[Bind("ProdutoId,Nome,DescricaoCurta,DescricapDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,ProdutoPreferido,EmEstoque,CategoriaId,Categoria")] Produto produto)
        {

            if (id != produto.ProdutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.Update(produto);
                    await _appDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.ProdutoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_appDbContext.Categorias, "CategoriaId", "CategoriaNome", produto.CategoriaId);
            return View(produto);
        }
        public IActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = _appDbContext.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_appDbContext.Categorias, "CategoriaId", "CategoriaNome", produto.CategoriaId);
            return View(produto);
        }
        public IActionResult Criar()
        {
            ViewData["CategoriaId"] = new SelectList(_appDbContext.Categorias, "CategoriaId", "CategoriaNome"); // Importantante para aparecer as opções da categoria
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("ProdutoId,Nome,DescricaoCurta,DescricapDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,ProdutoPreferido,EmEstoque,CategoriaId,Categoria")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Add(produto);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_appDbContext.Categorias, "CategoriaId", "CategoriaNome");
            return View(produto);
        }
        public IActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = _appDbContext.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return BadRequest();

            }

            return View(produto);


        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {


            var produto = _appDbContext.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            _appDbContext.Produtos.Remove(produto);

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));


        }
        private bool ProdutoExists(int id)
        {
            return _appDbContext.Produtos.Any(e => e.ProdutoId == id);
        }



    }

    }

