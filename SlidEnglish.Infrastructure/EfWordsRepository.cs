using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;
using SlidEnglish.App;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Infrastructure
{
    public class EfWordsRepository : EfRepository<Word, int>, IRepositoryWithAccessCheck<Word>
    {
        public EfWordsRepository(ApplicationDbContext dbContext) : base(dbContext) {}

        public async Task<List<Word>> GetListWithAccessCheck(string userId)
        {
            return await _dbContext.Words.Where(x => x.User.Id == userId).ToListAsync();
        }

		public async Task<Word> GetByIdWithAccessCheck(string userId, int id)
		{
			return await _dbContext.Words.FirstOrDefaultAsync(x => x.User.Id == userId && x.Id == id);
		}
    }
}
