using AutoMapper;
using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SlidEnglish.App
{
	public class TranslateService
	{
		private IDataAccessLayer _dal;
		private IMapper _mapper;

        public TranslateService(IDataAccessLayer dal, IMapper mapper)
		{
			_dal = dal;
			_mapper = mapper;
        }

        public async Task ProcessTranslate(string userId, string text)
        {
            var words = Split(text);

            foreach (var word in words)
            {
                var existWord = await _dal.Words.GetByTextWithAccessCheck(userId, word);

                if (existWord == null)
                    await AddAsync(userId, word);
                else
                {
                    // TODO: Add statistics of usages
                    existWord.Usages++;
                    await _dal.Words.Update(existWord);
                }
            }
        }

        public string[] Split(string text)
        {
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = text.Split().Select(x => x.Trim(punctuation)).ToArray();

            return words.Length > 2 ? words : new string[] { text };
        }

        public async Task AddAsync(string userId, string word)
        {
            var newWord = new Word(word)
            {
                User = await _dal.Users.GetById(userId),
                Attributes = WordAttribute.TranslateInput
            };

            await _dal.Words.Add(newWord);
        }
    }
}
