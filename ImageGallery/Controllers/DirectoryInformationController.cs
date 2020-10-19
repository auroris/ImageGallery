using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Controllers
{
    [Route("api/dir")]
    [ApiController]
    public class DirectoryInformationController : ControllerBase
    {
        // GET: /api/DirectoryInfo
        [HttpGet]
        public IActionResult Get()
        {
            return Get("");
        }

        // GET: /api/DirectoryInfo/dir
        [HttpGet("{*requestPath}", Name = "Get")]
        public IActionResult Get(String requestPath)
        {
            String reqPath = requestPath.Replace('/', Path.DirectorySeparatorChar);
            String realPath = Path.Combine(Startup.Configuration.GetValue<String>("GalleryPath"), reqPath);
            
            // The requested path must be a subdir of our configured gallery path, and it must exist
            if (requestPath.Length == 0 || (realPath.Contains(reqPath) && Directory.Exists(realPath)))
            {
                return Ok(new Models.DirectoryInfo(realPath));
            }

            return NotFound();
        }
    }
}
