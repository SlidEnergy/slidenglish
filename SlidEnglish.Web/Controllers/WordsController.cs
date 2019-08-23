using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlidEnglish.Web
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly WordsService _wordsService;

        public WordsController(IMapper mapper, WordsService wordsService)
        {
            _mapper = mapper;
            _wordsService = wordsService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<App.Dto.Word[]>> GetList()
        {
            var userId = User.GetUserId();

			return await _wordsService.GetListAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<App.Dto.Word>> Add(App.Dto.Word word)
        {
            var userId = User.GetUserId();

            var newBank = await _wordsService.AddAsync(userId, word);

            return CreatedAtAction("GetList", newBank);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<App.Dto.Word>> Update(int id, App.Dto.Word word)
        {
            var userId = User.GetUserId();

            return await _wordsService.UpdateAsync(userId, word);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.GetUserId();

            await _wordsService.DeleteAsync(userId, id);

            return NoContent();
        }
    }
}
