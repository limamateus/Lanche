using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lanche.Models
{
    [Table("TbPedidos")]
    public class TbPedido
    {   [Key]
        public int TbPedidoId { get; set; }

        [Required(ErrorMessage = "Informe o Nome")]
        [StringLength(50)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe o Sobrenome")]
        [StringLength(50)]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Informe o seu endereço")]
        [StringLength(100)]
        [Display(Name = "Endereço")]
        public string Endereco1 { get; set; }

        [StringLength(100)]
        [Display(Name = "Complemento")]
        public string Endereco2 { get; set; }

        [Required(ErrorMessage = "Informe seu  Cep")]
        [Display(Name = "CEP")]
        [StringLength(10, MinimumLength = 8)]
        public string Cep { get; set; }

        [StringLength(10)]
        public string Estado { get; set; }

        [StringLength(50)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Informe o seu telefone")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Informe o seu telefone")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total do Pedido")]
        public decimal PedidoTotal { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Itens no Pedido")]
        public int TotalItensPedido { get; set; }

        [Display(Name = "Data do Pedido")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}")]
        public DateTime PedidoEnviado { get; set; }

        [Display(Name = "Data de Envio do Pedido")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}")]
        public DateTime? PedidoEntregueEm { get; set; }

        public virtual List<TbPedidoDetalhe> TbPedidoItens { get; set; }


    }
}
