using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.Domain;
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
        public async Task<ActionResult<Dto.Word[]>> GetList()
        {
            var userId = User.GetUserId();

            var banks = await _wordsService.GetListAsync(userId);
            return _mapper.Map<Dto.Word[]>(banks);
        }

        [HttpPost]
        public async Task<ActionResult<Dto.Word>> Add(Dto.Word word)
        {
            var userId = User.GetUserId();

            var newBank = await _wordsService.AddAsync(userId, _mapper.Map<Word>(word));

            return CreatedAtAction("GetList", _mapper.Map<Dto.Word>(newBank));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Dto.Word>> Update(int id, Dto.Word word)
        {
            var userId = User.GetUserId();

            var editedBank = await _wordsService.EditAsync(userId, _mapper.Map<Word>(word));
            return _mapper.Map<Dto.Word>(editedBank);
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
