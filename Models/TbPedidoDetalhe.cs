using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Lanche.Models
    {
    [Table("TbPedidoDetalhes")]
    public class TbPedidoDetalhe
        {
            [Key]
            public int TbPedidoDetalheId { get; set; }

            public int TbPedidoId { get; set; }

            public int ProdutoId { get; set; }

            public int Quantidade { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal Preco { get; set; }


            public virtual Produto Produto { get; set; }

            public virtual TbPedido TbPedido { get; set; }
        }
    }



