using GraphQL;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Graphql
{
	public class Query
	{
		private WordsService _wordsService;
		private string _userId;

		public Query(WordsService wordsService, string userId)
		{
			_wordsService = wordsService;
			_userId = userId;
		}


		[GraphQLMetadata("words")]
		public async Task<Word[]> Words()
		{
			return await _wordsService.GetListAsync(_userId);
		}
	}
}
