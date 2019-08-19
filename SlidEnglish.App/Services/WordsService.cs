using AutoMapper;
using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SlidEnglish.App
{
	public class WordsService
	{
		private IDataAccessLayer _dal;
		private IMapper _mapper;

		public WordsService(IDataAccessLayer dal, IMapper mapper)
		{
			_dal = dal;
			_mapper = mapper;
		}

		public async Task<Dto.Word> AddAsync(string userId, Dto.Word word)
		{
			var newWord = _mapper.Map<Word>(word);

			newWord.User = await _dal.Users.GetById(userId);
			newWord.Sinonyms = new List<WordSinonym>(word.Synonyms.Length);

			foreach (var synonymId in word.Synonyms)
			{
					var linkedWord = await _dal.Words.GetByIdWithAccessCheck(userId, synonymId);
					newWord.Sinonyms.Add(new WordSinonym(newWord, linkedWord));
			}

			await _dal.Words.Add(newWord);

			return _mapper.Map<Dto.Word>(newWord);
		}

		public Task<Word> GetAsync(string userId, int id)
		{
			return _dal.Words.GetByIdWithAccessCheck(userId, id);
		}

		public async Task<Dto.Word[]> GetListAsync(string userId)
		{
			var words = await _dal.Words.GetListWithAccessCheck(userId);

			return _mapper.Map<Dto.Word[]>(words);
		}

		public async Task<Dto.Word> EditAsync(string userId, Dto.Word word)
		{
			var editWord = await _dal.Words.GetByIdWithAccessCheck(userId, word.Id);
			// вызываем чтобы сработал lazyloading, это позволит потом сохранить это свойство
			var oldSynonyms = editWord.Sinonyms;
			var oldSynonymsOf = editWord.SinonymOf;

			_mapper.Map<Dto.Word, Word>(word, editWord);

			var synonyms = new List<WordSinonym>(word.Synonyms.Length);
			var synonymsOf = new List<WordSinonym>(word.Synonyms.Length);

			foreach (var synonymId in word.Synonyms)
			{
				if (editWord.SinonymOf.Any(x => x.WordId == synonymId && x.SinonymId == editWord.Id))
				{
					var linkedWord = await _dal.Words.GetByIdWithAccessCheck(userId, synonymId);
					synonymsOf.Add(new WordSinonym(linkedWord, editWord));
				}
				else
				{
					var linkedWord = await _dal.Words.GetByIdWithAccessCheck(userId, synonymId);
					synonyms.Add(new WordSinonym(editWord, linkedWord));
				}
			}

			editWord.Sinonyms = synonyms;
			editWord.SinonymOf = synonymsOf;

			await _dal.SaveChangesAsync();

			return _mapper.Map<Dto.Word>(editWord);
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
