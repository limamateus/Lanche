
using Lanche.Context;
using Microsoft.EntityFrameworkCore;

namespace Lanche.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }

        public string CarrinhoCompraId { get; set; }

        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider service)
        {
                       //Definir uma sessão
                        ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            //obeter um serviço do contexto
            var context = service.GetService<AppDbContext>();

            string carrinhoId = session.GetString("CarrinhoId")?? Guid.NewGuid().ToString();

            session.SetString("CarrinhoId", carrinhoId);
           

            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };


           
        }
        public void AdicionarAoCarrinho(Produto produto)
        { // Condição que vai verificar se o produto e carrinho já existe, uma consulta linq
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault
                (s => s.Produto.ProdutoId == produto.ProdutoId  
                && s.CarrinhoCompraId == CarrinhoCompraId);
            // essa condição vai veririca se o carrinho é vazio e adicionar o produto e quantidade 1
            //caso contrario ele vai somente adicionar a quantidade

            if(carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Produto = produto,
                    Quantidade = 1
                };

                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);

            }
            else
            {
                carrinhoCompraItem.Quantidade++;

            }

           _context.SaveChanges();


        }

        public int RemoverDoCarrinho(Produto produto)
        {// Realizando uma consulta para saber se tem ou não um carrionho
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Produto.ProdutoId == produto.ProdutoId 
                && s.CarrinhoCompraId == CarrinhoCompraId);

           var quantidadeLocal = 0;

            if(carrinhoCompraItem != null)
            {
                if(carrinhoCompraItem.Quantidade > 1)
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;

                }else
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }

                

            }

            _context.SaveChanges();
            return quantidadeLocal;
        }

        public List<CarrinhoCompraItem> GetCarrinhoCompraItems()
        {
            // Vai verificar o CarrinhoCompra Itens verificar se é null se for igual
            // ele vai fazer um consultar no banco onde todos os id forem iguais e trazer
            // todos os produtos em forma de lista
            return CarrinhoCompraItens ?? (CarrinhoCompraItens = _context.CarrinhoCompraItens.
                Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                .Include(s => s.Produto).ToList());
        }

        public void LimparCarrinho()
        { // criando uma variavel de consulta que vai me trazer o carrinho
            var carrinhoItens = _context.CarrinhoCompraItens.
                Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);
            // e usar o medodo do NET para remover todos.
            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            // e salvar no banco
            _context.SaveChanges();

        }


        public decimal GetCarrinhoCompraTotal()
        {
            // vou amarzenar na variavel um consulta linq
            // onde vou consultar meu carrionho  selecionar os produtos e mutiplicar
            // a quantidade deles para e trazer a soma deles retornar na variavel total.
            var total = _context.CarrinhoCompraItens.
                Where(carrinho => carrinho.CarrinhoCompraId==CarrinhoCompraId).
                Select(c=> c.Produto.Preco * c.Quantidade)
                .Sum();
            return total;
        }
    }


}
