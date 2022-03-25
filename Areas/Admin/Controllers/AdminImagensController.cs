using Lanche.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Lanche.Areas.Admin.Controllers
{
       [Area("Admin")]
       [Authorize(Roles ="Admin")]
    public class AdminImagensController : Controller
    {

        private readonly ConfigurationImagens _myConfig;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminImagensController( IWebHostEnvironment hostingEnvironment,IOptions<ConfigurationImagens> myConfiguration)
        {
           
            _hostingEnvironment = hostingEnvironment;
            _myConfig = myConfiguration.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
     
        [HttpPost]       
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            if(files == null || files.Count == 0)// Verificando se o arquivo é null ou não tem nada
            {
                ViewData["Erro"] = "Erro: Arquivo(s) não selecionado(s) ";
                return View(ViewData);
            }

            if(files.Count > 10) // verificação se a quantidade arquivo for maior que 10, se for vai trazer na view a mensagem de erro destacado em baixa
            {
                ViewData["Erro"] = "Erro: Quantidade de arquivo excedeu o limete ";
                return View(ViewData);
            }

            long size   = files.Sum(f => f.Length); // variavel que vai somar o tamanho dos arquivos

            var filePathsName = new List<string>(); // variavel que vai amazernar o nome das imagens

            var filePaths = Path.Combine(_hostingEnvironment.WebRootPath,_myConfig.NomePastaImagensProdutos);


            foreach( var formfile in files) // voou percorrer os arquivos selecionados 
            {
                // verificar se todos os arquivos são jpg, ou gif ou png
                if (formfile.FileName.Contains(".jpg")|| formfile.FileName.Contains(".gif") || formfile.FileName.Contains(".png"))
                {
                    var fileNameWithPath = string.Concat(filePaths,"\\",formfile.FileName); 
                    // criar na pasta o arquivo 
                    using(var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await formfile.CopyToAsync(stream);
                    }
                }
            }
            // e passar a mensagem na viewdata
            ViewData["Resultado"] = $"{files.Count} arquivo foram enviado ao servidor, " +
                $"com tamanho total de : {size} bytes";

            ViewBag.Arquivos = filePathsName;

            return View(ViewData);

        }


        public async Task<IActionResult> GetImagens()
        {// eu crio uma stancia do model
            FileManagerModel model = new FileManagerModel();
            // faço um consulta do caminho e atribui a varia vel 
            var userImagensPath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos);
            // crio uma instacia usado a class Directoryinfo da bibliote IO
            DirectoryInfo dir = new DirectoryInfo(userImagensPath);

            FileInfo[] files = dir.GetFiles(); // uso esse metodo para pegar os arquivos 

            model.PathImagensProduto = _myConfig.NomePastaImagensProdutos; // atribou o caminho dapasta

            if(files.Length == 0)
            {
                ViewData["Erro"] = $"Nenhum arquivo foi encontrado {userImagensPath} ";
                
            } 

            model.Files = files; // e defino o nome do arquivo que o usuario selecionou e envio para view

            return View(model);


        }


        public IActionResult Deletefile(string fname)
        {
            string _imagemDeleta = Path.Combine(_hostingEnvironment.WebRootPath,_myConfig.NomePastaImagensProdutos + "\\" , fname);

            if (System.IO.File.Exists(_imagemDeleta))
            {
                System.IO.File.Delete(_imagemDeleta);

                ViewData["Deletado"] = $"Arquivo(s) {_imagemDeleta} Deletada com sucesso";
            }

            return View("Index");
        }
    }
}
