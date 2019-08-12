using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface IRefreshTokensRepository
    {
        Task<RefreshToken> GetByUserId(string userId);

		Task<RefreshToken> Add(RefreshToken entity);

		Task<RefreshToken> Update(RefreshToken entity);
    }
}
