using Lanche.Models;
using Lanche.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lanche.Context;
using System;

namespace Lanche.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
