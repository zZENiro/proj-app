using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using view_controller_application.Commands;

namespace view_controller_application.Contollers
{
    [Controller]
    [Route("api")]
    public class FileController : Controller
    {
        private readonly IDistributedCache _cache;

        private static int _imageCounter = 0;

        public FileController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            var data = new byte[file.Length];

            using (var bstream = file.OpenReadStream())
            {
                await bstream.ReadAsync(data, cancellationToken);
            }

            await _cache.SetAsync((++_imageCounter).ToString(), data, cancellationToken);

            return Ok(_imageCounter);
        }

        [HttpGet("load")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> LoadFiles([FromQuery] int? id)
        {
            var data = await _cache.GetAsync(id.ToString());
            
            return File(data, "image/png");
        }

        [HttpGet("image")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> GetImage()
        {
            var data = await _cache.GetAsync("1");
            return Ok(new FileContentResult(data, "image/png"));            
        }

        [HttpGet("check")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Check()
        {
            var item = await _cache.GetAsync("1");
            return Ok(item == null ? false : true);
        }
    }
}
