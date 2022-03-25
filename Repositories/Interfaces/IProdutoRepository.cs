using Lanche.Models;

namespace Lanche.Repositories.Interfaces
{
    public interface IProdutoRepository
    { 

        IEnumerable<Produto> Produtos { get; }

        IEnumerable<Produto> ProdutoPreferido { get; }

        Produto GetById (int Produtoid);
           
    }
}
