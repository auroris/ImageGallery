using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Controllers
{
    [Route("api/Image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // GET: /api/Image
        [HttpGet("{*requestPath}", Name = "GetImage")]
        public IActionResult GetImage(String requestPath)
        {
            String reqPath = requestPath.Replace('/', Path.DirectorySeparatorChar);
            String realPath = Path.Combine(Startup.Configuration.GetValue<String>("GalleryPath"), reqPath);

            if (realPath.Contains(reqPath) && System.IO.File.Exists(realPath))
            {
                Byte[] b = System.IO.File.ReadAllBytes(realPath);
                return File(b, "image/jpeg");
            }

            throw new Exception("Invalid path");
        }
    }
}