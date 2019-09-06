using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public class ImportService : IImportService
    {
        private IApplicationDbContext _context;

        public ImportService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ImportMultiple(string userId, string text)
        {
            var lexicalUnits = text.Split('\n');

            foreach (var lexicalUnit in lexicalUnits)
            {
                await ImportSingle(userId, lexicalUnit);
            }
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
                InputAttributes = LexicalUnitInputAttribute.UserInput
            };

            _context.LexicalUnits.Add(newLexicalUnit);
            await _context.SaveChangesAsync();
        }
    }
}
