using Lanche.Context;
using Lanche.Models;
using Microsoft.EntityFrameworkCore;

namespace Lanche.Areas.Admin.Services
{
    public class RelatorioVendasServices
    {

        private readonly AppDbContext _appDbContext;

        public RelatorioVendasServices(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<List<TbPedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in _appDbContext.TbPedidos select obj; // Consulta inicial

            if (minDate.HasValue) // conndição que vai verificar o tem uma data inicial
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value); 
            }
            if (maxDate.HasValue)
            {
                resultado.Where(x => x.PedidoEnviado <= maxDate.Value);
            }

            return await resultado.Include(l => l.TbPedidoItens).ThenInclude(l=> l.Produto)
                .OrderByDescending(x => x.PedidoEnviado).ToListAsync();


        }
    }
}
