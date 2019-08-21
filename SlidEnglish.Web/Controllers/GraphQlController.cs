using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.Web.GraphQL;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Controllers
{
	[Authorize]
	[Route("graphql")]
	[ApiController]
	public class GraphQlController : ControllerBase
	{
		private WordsService _wordsService;

		public GraphQlController(WordsService wordsService)
		{
			_wordsService = wordsService;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] GraphQlQuery query)
		{
			var userId = User.GetUserId();

			var inputs = query.Variables.ToInputs();

			var schema = new Schema
			{
				Query = new WordQuery(_wordsService, userId),
				Mutation = new WordMutation(_wordsService, userId)
			};

			var result = await new DocumentExecuter().ExecuteAsync(c =>
			{
				c.Schema = schema;
				c.Query = query.Query;	
				c.OperationName = query.OperationName;
				c.Inputs = inputs;
			});

			if (result.Errors?.Count > 0)
			{
				return BadRequest();
			}

			return Ok(result);
		}
	}
}
