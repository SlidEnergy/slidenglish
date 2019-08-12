using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
	public class WordsService
	{
		private IDataAccessLayer _dal;

		public WordsService(IDataAccessLayer dal)
		{
			_dal = dal;
		}

		public async Task<Word> AddAsync(string userId, Word word)
		{
			if (word.User == null)
			{
				var user = await _dal.Users.GetById(userId);
				word.User = user;
			}

			await _dal.Words.Add(word);

			return word;
		}

		public Task<Word> GetAsync(string userId, int id)
		{
			return _dal.Words.GetByIdWithAccessCheck(userId, id);
		}

		public Task<List<Word>> GetListAsync(string userId)
		{
			return _dal.Words.GetListWithAccessCheck(userId);
		}

		public async Task<Word> EditAsync(string userId, Word word)
		{
			var editWord = await _dal.Words.GetByIdWithAccessCheck(userId, word.Id);

			if (editWord == null)
				throw new EntityNotFoundException();

			editWord.Text = word.Text;

			await _dal.SaveChangesAsync();

			return word;
		}

		public async Task<bool> ExistsAsync(string userId, Word word) => await _dal.Words.GetByIdWithAccessCheck(userId, word.Id) != null;

		public async Task DeleteAsync(string userId, int id)
		{
			var word = await _dal.Words.GetByIdWithAccessCheck(userId, id);

			if (word == null)
				throw new EntityNotFoundException();

			await _dal.Words.Delete(word);
		}
	}
}
