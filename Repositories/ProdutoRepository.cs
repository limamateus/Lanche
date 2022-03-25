using Lanche.Context;
using Lanche.Models;
using Lanche.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lanche.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {

        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Produto> Produtos => _context.Produtos.Include(c => c.Categoria);

        public IEnumerable<Produto> ProdutoPreferido => _context.Produtos.Where(l=>l.ProdutoPreferido).Include(c=>c.Categoria);

        public Produto GetById(int produtoId)
        {
            return _context.Produtos.FirstOrDefault(l => l.ProdutoId == produtoId);
        }
    }
}
