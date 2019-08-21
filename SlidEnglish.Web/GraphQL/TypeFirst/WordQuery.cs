using GraphQL.Types;
using SlidEnglish.App;

namespace SlidEnglish.Web.Graphql
{
	public class WordQuery : ObjectGraphType
	{
		public WordQuery(WordsService wordsService, string userId)
		{
			Field<ListGraphType<WordType>>("Words", resolve: context => wordsService.GetListAsync(userId));
		}
	}
}
