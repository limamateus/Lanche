﻿using Lanche.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lanche.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
        public DbSet<TbPedido> TbPedidos { get; set; }

        public DbSet<TbPedidoDetalhe> TbPedidoDetalhes { get; set; }


    }



}
