using SlidEnglish.App;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.Infrastructure
{
	public class DataAccessLayer : IDataAccessLayer
	{
		private readonly ApplicationDbContext _context;

        public IRefreshTokensRepository RefreshTokens { get; }
        public ILexicalUnitsRepository LexicalUnits { get; }
		public IRepository<User, string> Users { get; }

		public DataAccessLayer(
			ApplicationDbContext context,
			IRepository<User, string> users,
            ILexicalUnitsRepository lexicalUnits,
            IRefreshTokensRepository refreshTokens)
		{
			_context = context;
			Users = users;
			LexicalUnits = lexicalUnits;
            RefreshTokens = refreshTokens;
        }

		public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
