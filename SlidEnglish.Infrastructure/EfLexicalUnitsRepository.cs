using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;
using SlidEnglish.App;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Infrastructure
{
    public class EfLexicalUnitsRepository : EfRepository<LexicalUnit, int>, ILexicalUnitsRepository
    {
        public EfLexicalUnitsRepository(ApplicationDbContext dbContext) : base(dbContext) {}

        public async Task<List<LexicalUnit>> GetListWithAccessCheck(string userId)
        {
            return await _dbContext.LexicalUnits.Where(x => x.User.Id == userId).ToListAsync();
        }

		public async Task<LexicalUnit> GetByIdWithAccessCheck(string userId, int id)
		{
			return await _dbContext.LexicalUnits.FirstOrDefaultAsync(x => x.User.Id == userId && x.Id == id);
		}

        public async Task<LexicalUnit> GetByTextWithAccessCheck(string userId, string text)
        {
            return await _dbContext.LexicalUnits.FirstOrDefaultAsync(x => x.User.Id == userId && x.Text == text);
        }
    }
}
