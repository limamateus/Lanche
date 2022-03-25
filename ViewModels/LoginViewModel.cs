using System.ComponentModel.DataAnnotations;

namespace Lanche.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Nome do Usuario")]
        [Required(ErrorMessage ="Informe o nome do usuario")]
        [StringLength(100)]
        public string UserName { get; set; }
    
        [Required(ErrorMessage ="Informe a senha do usuario")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha do Usuario")]
        public string Password { get; set; }


        public string ReturnUrl { get; set; }

    }

}
