using GraphQL;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System.Linq;
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
		public async Task<object[]> Words()
		{
			var words = await _wordsService.GetListAsync(_userId);
			return words.Select(x => new {
				Id = x.Id,
				Text = x.Text,
				Association = x.Association,
				Description = x.Description,
				Synonyms = words.Where(w => x.Synonyms.Contains(w.Id))
			}).ToArray();
		}
	}
}
