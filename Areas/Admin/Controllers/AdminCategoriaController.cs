using Microsoft.AspNetCore.Mvc;
using Lanche.Models;
using Microsoft.AspNetCore.Authorization;
using Lanche.Context;
using Microsoft.EntityFrameworkCore;
using Lanche.ViewModels;

namespace Lanche.Areas.Admin.Controllers
{
    [Authorize("Admin")]
    [Area("Admin")]
    public class AdminCategoriaController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AdminCategoriaController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.Categorias.ToList());
        }
      
        public IActionResult Editar(int? id )

        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, [Bind("CategoriaId,CategoriaNome,Descricao")] Categoria categoria)
       {
            if(id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.Categorias.Update(categoria);
                    _appDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
                return RedirectToAction(nameof(Index)); 
            }

            return View(categoria);
        }
        

        public IActionResult Detalhes(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if(categoria == null)
            {
                return NotFound();
            }


            return View(categoria);
        }


        public  IActionResult Criar()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("CategoriaId,CategoriaNome,Descricao")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Add(categoria);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        public IActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return BadRequest();

            }

            return View(categoria);


        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            _appDbContext.Categorias.Remove(categoria);

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));


        }

       

    }
}
