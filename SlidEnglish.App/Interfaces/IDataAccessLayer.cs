using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
	public interface IDataAccessLayer
    {
        IWordsRepository Words { get; }
		IRepository<User, string> Users { get; }
        IRefreshTokensRepository RefreshTokens { get; }

        Task SaveChangesAsync();
    }
}
