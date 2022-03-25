using Lanche.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lanche.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl) // Metodo Get 
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) // verificando se o login foi preenchido 
            {
                return View(loginVM);
            }

            var user = await _userManager.FindByNameAsync(loginVM.UserName); // consultar o ususario no bando e atribuir a variavael loca
            
            if(user != null) // se o usuario for diferente de null, quer dizer ele existe, então vou consultar a senha e definir para não persistir o cookie
                             // ou seja, se ele sair ele vai entra com usuario e senha de novo e caso a sessão expira não vai bloquear o usuario
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                
                if (result.Succeeded) // se o login foi feito com sucesso eu fazer a senguinte condição
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl)) // se ele não estiver em nenhuma link ele vai ser redirecionado para Index
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl); // caso contrario ele vai ficar na tela que ele já está
                }


            }
            // Caso de alguma falha no login ele vai aparecer essa msg de erro para usuario e voltar para tela de login
            ModelState.AddModelError("", "Erro ao realizar o Login, verifique o cadastro digitado");
            return View(loginVM);
            
        }

        [HttpGet]
        public IActionResult Register() // Metodo para entra na minha tela de registo
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(LoginViewModel registroVM)
        {
            if (ModelState.IsValid) // verificando se fou valido a model
            {
                var user = new IdentityUser() { UserName = registroVM.UserName }; // criando uma novo usuario
                var result = await _userManager.CreateAsync(user, registroVM.Password);

                if (result.Succeeded) // verificando se foi criado com sucesso e redirecionando para tela de login
                {
                    await _userManager.AddToRoleAsync(user, "Member");// definindo que todos que se cadastra aqui vai ser usuario cliente
                    return RedirectToAction("Login", "Account"); // 
                } else
                {

                    this.ModelState.AddModelError("Registro", "Falha ao registar");
                }
            }

            return View(registroVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();//Remover todos os valores (conteudo) dos Objetos na Session
            HttpContext.User = null; // não atribuira um valor 
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); //("Nome da View", "Controlador")
        }

        public  IActionResult AccessDenied()
        {
            return View();
        }

    }
}
