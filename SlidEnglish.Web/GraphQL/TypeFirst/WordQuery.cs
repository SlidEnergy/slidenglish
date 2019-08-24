using GraphQL.Types;
using SlidEnglish.App;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Graphql
{
	public class WordQuery : ObjectGraphType
	{
		public WordQuery(WordsService wordsService, string userId)
		{
			Field<ListGraphType<WordType>>("Words", resolve: context =>
            {
                var words = Task.Run(async () => await wordsService.GetListAsync(userId)).Result;
                //var words = await wordsService.GetListAsync(userId);
                return words.Select(x => new WordGraphql
                {
                    Id = x.Id,
                    Text = x.Text,
                    Association = x.Association,
                    Description = x.Description,
                    Synonyms = words.Where(w => x.Synonyms.Contains(w.Id)).Select(w => new WordGraphql
                    {
                            Id = w.Id,
                            Text = w.Text,
                            Association = w.Association,
                            Description = w.Description
                        }).ToArray()
                    }).ToArray();
            });
		}
	}
}
