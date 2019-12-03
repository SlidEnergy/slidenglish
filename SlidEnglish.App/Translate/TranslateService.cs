using AutoMapper;
using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SlidEnglish.App
{
    public class TranslateService : ITranslateService
    {
        private IApplicationDbContext _context;
        private ITranslator _translator;

        public TranslateService(IApplicationDbContext context, ITranslator translator)
        {
            _context = context;
            _translator = translator;
        }

        public async Task<Dto.TranslateData> ProcessTranslate(string userId, string text)
        {
            var lexicalUnits = Split(text);

            foreach (var lexicalUnit in lexicalUnits)
            {
                await ImportSingle(userId, lexicalUnit);
            }

            var translatedText = await _translator.TranslateAsync(text);

            return new Dto.TranslateData { Text = translatedText };
        }

        private string[] Split(string text)
        {
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            var lexicalUnits = text.Split().Select(x => x.Trim(punctuation)).ToArray();

            return lexicalUnits.Length > 2 ? lexicalUnits : new string[] { text };
        }

        public async Task ImportSingle(string userId, string lexicalUnit)
        {
            var existLexicalUnit = await _context.LexicalUnits.ByUser(userId).FirstOrDefaultAsync(x => x.Text == lexicalUnit);

            if (existLexicalUnit == null)
                await AddAsync(userId, lexicalUnit);
            else
                await UpdateAsync(existLexicalUnit);
        }

        private async Task UpdateAsync(LexicalUnit lexicalUnit)
        {
            lexicalUnit.UsagesCount++;
            _context.LexicalUnits.Update(lexicalUnit);
            await _context.SaveChangesAsync();
        }

        private async Task AddAsync(string userId, string lexicalUnit)
        {
            var newLexicalUnit = new LexicalUnit
            {
                Text = lexicalUnit,
                User = await _context.Users.FindAsync(userId),
                InputAttributes = LexicalUnitInputAttribute.TranslateInput
            };

            _context.LexicalUnits.Add(newLexicalUnit);
            await _context.SaveChangesAsync();
        }
    }
}
