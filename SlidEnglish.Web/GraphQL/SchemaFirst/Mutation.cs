using GraphQL;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
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
		public async Task<Word> AddWord(Word word)
		{
			return await _wordsService.AddAsync(_userId, word);
		}

		[GraphQLMetadata("updateWord")]
		public async Task<Word> UpdateWord(Word word)
		{
			return await _wordsService.EditAsync(_userId, word);
		}

		[GraphQLMetadata("deleteWord")]
		public async Task<bool> DeleteWord(int id)
		{
			await _wordsService.DeleteAsync(_userId, id);
			return true;
		}
	}
}
