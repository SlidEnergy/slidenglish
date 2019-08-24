using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.Web.Graphql;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Controllers
{
	[Authorize]
	[Route("typefirstgraphql")]
	[ApiController]
	public class TypeFirstGraphQlController : ControllerBase
	{
		private WordsService _wordsService;
		private readonly IDependencyResolver _dependencyResolver;

		public TypeFirstGraphQlController(WordsService wordsService, IDependencyResolver dependencyResolver)
		{
			_wordsService = wordsService;
			_dependencyResolver = dependencyResolver;
		}
		[HttpPost]
		public async Task<ActionResult<ExecutionResult>> Post([FromBody] GraphQlQuery query)
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
