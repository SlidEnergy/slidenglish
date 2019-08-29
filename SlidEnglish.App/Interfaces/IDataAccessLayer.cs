using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
	public interface IDataAccessLayer
    {
        ILexicalUnitsRepository LexicalUnits { get; }
		IRepository<User, string> Users { get; }
        IRefreshTokensRepository RefreshTokens { get; }

        Task SaveChangesAsync();
    }
}
