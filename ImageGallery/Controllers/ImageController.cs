using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Workers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private List<String> validExt = new List<String>();
        private String galleryPath = Startup.Configuration.GetValue<String>("GalleryPath");

        public ImageController()
        {
            validExt.AddRange(Startup.Configuration.GetSection("Videos").Get<string[]>());
            validExt.AddRange(Startup.Configuration.GetSection("Images").Get<string[]>());
        }

        // GET: /api/image/full/path
        [HttpGet("full/{*requestPath}", Name = "GetImage")]
        public IActionResult GetImage(String requestPath)
        {
            String reqPath = requestPath.Replace('/', Path.DirectorySeparatorChar);
            String realPath = Path.Combine(galleryPath, reqPath);

            if (ValidateRequest(realPath))
            {
                return PhysicalFile(realPath, Mime.Types[Path.GetExtension(realPath).Trim('.')]);
            }

            return NotFound();
        }

        // GET: /api/image/thumb/path
        [HttpGet("thumb/{*requestPath}", Name = "GetThumbnail")]
        public IActionResult GetThumbnail(String requestPath)
        {
            String reqPath = requestPath.Replace('/', Path.DirectorySeparatorChar);
            String realPath = Path.Combine(galleryPath, reqPath);

            if (ValidateRequest(realPath))
            {
                return File(ImageWorker.GetResized(
                        realPath,
                        "thumbnail",
                        Startup.Configuration.GetValue<int>("Thumbnail:width"), 
                        Startup.Configuration.GetValue<int>("Thumbnail:height")
                    ), Mime.Types[Path.GetExtension(realPath).Trim('.')]);
            }

            return NotFound();
        }

        // GET: /api/image/large/path
        [HttpGet("large/{*requestPath}", Name = "GetLarge")]
        public IActionResult GetLarge(String requestPath)
        {
            String reqPath = requestPath.Replace('/', Path.DirectorySeparatorChar);
            String realPath = Path.Combine(galleryPath, reqPath);

            if (ValidateRequest(realPath))
            {
                return File(ImageWorker.GetResized(
                        realPath,
                        "large",
                        Startup.Configuration.GetValue<int>("Large:width"),
                        Startup.Configuration.GetValue<int>("Large:height")
                    ), Mime.Types[Path.GetExtension(realPath).Trim('.')]);
            }

            return NotFound();
        }

        private bool ValidateRequest(String path)
        {
            String ext = Path.GetExtension(path).ToLower().Trim('.');

            return path.Contains(Startup.Configuration.GetValue<String>("GalleryPath")) 
                && System.IO.File.Exists(path) 
                && validExt.Contains(ext);
        }
    }
}