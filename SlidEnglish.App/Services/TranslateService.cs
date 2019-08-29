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
            var lexicalUnits = Split(text);

            foreach (var lexicalUnit in lexicalUnits)
            {
                var existLexicalUnit = await _dal.LexicalUnits.GetByTextWithAccessCheck(userId, lexicalUnit);

                if (existLexicalUnit == null)
                    await AddAsync(userId, lexicalUnit);
                else
                {
                    // TODO: Add statistics of usages
                    existLexicalUnit.UsagesCount++;
                    await _dal.LexicalUnits.Update(existLexicalUnit);
                }
            }
        }

        public string[] Split(string text)
        {
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            var lexicalUnits = text.Split().Select(x => x.Trim(punctuation)).ToArray();

            return lexicalUnits.Length > 2 ? lexicalUnits : new string[] { text };
        }

        public async Task AddAsync(string userId, string lexicalUnit)
        {
            var newLexicalUnit = new LexicalUnit
            {
                Text = lexicalUnit,
                User = await _dal.Users.GetById(userId),
                InputAttributes = LexicalUnitInputAttribute.TranslateInput
            };

            await _dal.LexicalUnits.Add(newLexicalUnit);
        }
    }
}
