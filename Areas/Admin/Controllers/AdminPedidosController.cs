using Lanche.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lanche.Models;
using Lanche.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Lanche.Areas.Admin.Controllers
{       [Area("Admin")]
        [Authorize("Admin")]
    public class AdminPedidosController : Controller
    {
        private readonly AppDbContext _appDbContex; 

        public AdminPedidosController(AppDbContext appDbContex)
        {
            _appDbContex = appDbContex;
        }

        //public IActionResult Index() // View que vai me trazer uma lista de pedidos
        //{
        //    var pedidos = _appDbContex.TbPedidos;
            
        //    return View(pedidos.ToList()); // transformando os pedidos em uma lista
        //}


        public async Task<IActionResult> Index(string filter, int pageindex = 1 , string sort ="Nome")
        {
            var resultado =  _appDbContex.TbPedidos.AsNoTracking().AsQueryable(); // consulta

            if (!string.IsNullOrWhiteSpace(filter)) // verificar se foi infomamdo 
            {
                resultado = resultado.Where(p => p.Nome.Contains(filter)); // e definir um outra consulta

            }
            // estou criando os dados paginados 
            var model = await PagingList.CreateAsync(resultado, 2, pageindex, sort, "Nome");

            model.RouteValue = new RouteValueDictionary { { "filter" , filter } }; // Rota que vai ser gerado e enviado para view


            return View(model);
        }

        public IActionResult PedidoProdutos(int? id)
        {
            var pedido = _appDbContex.TbPedidos
                .Include(pd => pd.TbPedidoItens)
                .ThenInclude(l => l.Produto)
                .FirstOrDefault(p => p.TbPedidoId == id);

            if (pedido == null)
            {
                Response.StatusCode = 404;
                return View("PedidoNotFound", id.Value);
            }

            PedidoProdutoViewModels pedidoProdutos = new PedidoProdutoViewModels()
            {
                TbPedido = pedido,
                TbPedidoDetalhe = pedido.TbPedidoItens

            };
            return View(pedidoProdutos);

        }
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _appDbContex.TbPedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("TbPedidoId,Nome,Sobrenome,Endereco1,Endereco2,Cep,Estado,Cidade,Telefone,Email,PedidoEnviado,PedidoEntregueEm")] TbPedido pedido)
        {
            if (id != pedido.TbPedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContex.Update(pedido);
                    await _appDbContex.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.TbPedidoId))
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
            return View(pedido);
        }

        public IActionResult Detalhes(int? id) // botão de detalhe
        {
            if(id == null) // vai veridicar se o botão é nulo se for vai retornar um pagina de not found que vou altera depois para enviar para uma pagina de erro
            {
                return NotFound();
            }
            // vou fazer um consular do pedido no banco para trazer o ID do pedido que o usuario selecionou e atribui na variavel pedidos
            var pedidos = _appDbContex.TbPedidos.FirstOrDefault(p => p.TbPedidoId == id); 

            if(pedidos == null)
            {
                return BadRequest();
            }
            return View(pedidos);
        }


        public IActionResult Criar()
        {


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Criar([Bind("TbPedidoId,Nome,Sobrenome,Endereco1,Endereco2,Cep,Estado,Cidade,Telefone,Email,PedidoEnviado,PedidoEntregueEm")] TbPedido tbPedido)
        {
            if (ModelState.IsValid)
            {
                _appDbContex.Add(tbPedido);
                _appDbContex.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbPedido);
        }

        public IActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = _appDbContex.TbPedidos.FirstOrDefault(p => p.TbPedidoId == id);

            if (pedido == null)
            {
                return BadRequest();

            }

            return View(pedido);


        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var pedido = _appDbContex.TbPedidos.FirstOrDefault(p => p.TbPedidoId == id);

            _appDbContex.TbPedidos.Remove(pedido);

            _appDbContex.SaveChanges();

            return RedirectToAction(nameof(Index));

           
        }

        private bool PedidoExists(int id)
        {
            return _appDbContex.TbPedidos.Any(e => e.TbPedidoId == id);
        }

    }
}
