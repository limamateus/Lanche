using System.Collections;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace Lanche.Models
{
    public class FileManagerModel
    {

        public FileInfo[] Files { get; set; }

        public IFormFile IFormFile { get; set; }

        public List<IFormFile> IFormFiles { get; set; }

        public string PathImagensProduto { get; set; }
    }
}
