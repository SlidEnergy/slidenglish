using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System.Threading.Tasks;

namespace SlidEnglish.Web
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Import([FromBody]ImportDataBindingModel data)
        {
            var userId = User.GetUserId();

            await _importService.ImportMultiple(userId, data.Text);

            return Ok();
        }
    }
}