using GraphQL;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Graphql
{
	public class Mutation
	{
		private WordsService _wordsService;
		private string _userId;

		public Mutation(WordsService wordsService, string userId)
		{
			_wordsService = wordsService;
			_userId = userId;
		}


		[GraphQLMetadata("addWord")]
		public async Task<object> AddWord(Word word)
		{
			var newWord = await _wordsService.AddAsync(_userId, word);
            var words = await _wordsService.GetListAsync(_userId);
            return new {
                Id = newWord.Id,
                Text = newWord.Text,
                Association = newWord.Association,
                Description = newWord.Description,
                Synonyms = words.Where(w => newWord.Synonyms.Contains(w.Id))
            };
        }

		[GraphQLMetadata("updateWord")]
		public async Task<object> UpdateWord(Word word)
		{
			var newWord = await _wordsService.UpdateAsync(_userId, word);
            var words = await _wordsService.GetListAsync(_userId);
            return new
            {
                Id = newWord.Id,
                Text = newWord.Text,
                Association = newWord.Association,
                Description = newWord.Description,
                Synonyms = words.Where(w => newWord.Synonyms.Contains(w.Id))
            };
        }

		[GraphQLMetadata("deleteWord")]
		public async Task<bool> DeleteWord(int id)
		{
			await _wordsService.DeleteAsync(_userId, id);
			return true;
		}
	}
}
