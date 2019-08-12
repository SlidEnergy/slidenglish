using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
	public interface IDataAccessLayer
    {
        IRepositoryWithAccessCheck<Word> Words { get; }
		IRepository<User, string> Users { get; }
        IRefreshTokensRepository RefreshTokens { get; }

        Task SaveChangesAsync();
    }
}
