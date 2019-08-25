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
	[Route("schemafirstgraphql")]
    [Route("graphql")]
	[ApiController]
	public class SchemaFirstGraphQlController : ControllerBase
	{
		private WordsService _wordsService;
		private readonly IDependencyResolver _dependencyResolver;

		public SchemaFirstGraphQlController(WordsService wordsService, IDependencyResolver dependencyResolver)
		{
			_wordsService = wordsService;
			_dependencyResolver = dependencyResolver;
		}
		[HttpPost]
		public async Task<ActionResult<ExecutionResult>> Post([FromBody] GraphQlQuery query)
		{
			var userId = User.GetUserId();

			var inputs = query.Variables.ToInputs();

			var schema = Schema.For(@"
				type Word {
					id: Int!
					text: String!
					association: String!
					description: String!
					synonyms: [Word]!
				}

				input wordInput {
					id: Int
					text: String!
					association: String
					description: String
					synonyms: [Int]
				}

				type Query {
					words: [Word]
				}

				type Mutation {
					addWord(word: wordInput): Word
					updateWord(word: wordInput): Word
					deleteWord(id: Int): Boolean
				}
				", c =>
				{
					c.DependencyResolver = new FuncDependencyResolver(type =>
					{
						if (type == typeof(Query))
							return new Query(_wordsService, userId);

						if (type == typeof(Mutation))
							return new Mutation(_wordsService, userId);
						
						return _dependencyResolver.Resolve(type);
					});
					c.Types.Include<Query>();
					c.Types.Include<Mutation>();
				});

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
