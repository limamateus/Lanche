using Lanche.Models;

namespace Lanche.ViewModels
{
    public class ProdutoListViewModel
    {

        public IEnumerable<Produto> Produtos { get; set; }

        public string CategoriaAtual { get; set; }
    }
}
