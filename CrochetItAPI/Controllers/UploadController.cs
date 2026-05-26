using CrochetItAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrochetItAPI.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly BlobService blobService;
        public UploadController(BlobService blobService)
        {
            this.blobService = blobService;
        }
        [HttpPost("generate-url")]
        public IActionResult GenerateUrl([FromBody] string fileName)
        {
            var sasUrl = blobService.GenerateUploadSas(fileName);
            return Ok(sasUrl);
        }
    }
}
