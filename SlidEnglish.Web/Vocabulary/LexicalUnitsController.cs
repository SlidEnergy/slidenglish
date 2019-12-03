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
    public class LexicalUnitsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILexicalUnitsService _lexicalUnitsService;

        public LexicalUnitsController(IMapper mapper, ILexicalUnitsService lexicalUnitsService)
        {
            _mapper = mapper;
            _lexicalUnitsService = lexicalUnitsService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<App.Dto.LexicalUnit[]>> GetList()
        {
            var userId = User.GetUserId();

			return await _lexicalUnitsService.GetListAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<App.Dto.LexicalUnit>> Add(App.Dto.LexicalUnit word)
        {
            var userId = User.GetUserId();

            var newBank = await _lexicalUnitsService.AddAsync(userId, word);

            return CreatedAtAction("GetList", newBank);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<App.Dto.LexicalUnit>> Update(int id, App.Dto.LexicalUnit lexicalUnit)
        {
            var userId = User.GetUserId();

            if (id != lexicalUnit.Id)
                return BadRequest();

            return await _lexicalUnitsService.UpdateAsync(userId, lexicalUnit);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.GetUserId();

            await _lexicalUnitsService.DeleteAsync(userId, id);

            return NoContent();
        }
    }
}
