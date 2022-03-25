using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lanche.Models
{
    [Table("Produtos")]
    public class Produto
    {

        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome do lanche deve ser informador")]
        [Display(Name = "Nome do Lanche")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = " O {0} deve ter no minimo {2} e o maximo {1} caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O nome do lanche deve ser informador")]
        [Display(Name = "Descricao Curta")]
        [MinLength(20, ErrorMessage = " Descrição deve ter no minimo {1}")]
        [MaxLength(200, ErrorMessage = "Descrição deve ter no maximo {1}")]
        public string DescricaoCurta { get; set; }

        [Required(ErrorMessage = "O nome do lanche deve ser informador")]
        [Display(Name = "Descrição detalhada")]
        [MinLength(20, ErrorMessage = " Descrição deve ter no minimo {1}")]
        [MaxLength(200, ErrorMessage = "Descrição deve ter no maximo {1}")]
        public string DescricapDetalhada { get; set; }

        [Required(ErrorMessage = "Informe o preço")]
        [Display(Name = "Preço")]
        [Column(TypeName = "Decimal(10,2)")]
        [Range(1, 9999.99, ErrorMessage = "O {0} deve ter entre 1 até 999,99")]
        public decimal Preco { get; set; }

        [Display(Name = "Caminho da Imagem")]
        [StringLength(200, ErrorMessage = " O {0} deve ter no maximo {1} caracter ")]
        public string ImagemUrl { get; set; }

        [Display(Name = "Caminho da Imagem em Url")]
        [StringLength(200, ErrorMessage = " O {0} deve ter no maximo {1} caracter ")]
        public string ImagemThumbnailUrl { get; set; }

        public bool ProdutoPreferido { get; set; }

        public bool EmEstoque { get; set; }

        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; }

    }
}
