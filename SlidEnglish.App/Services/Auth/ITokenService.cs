using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.App
{
    public interface ITokenService
    {
        Task<RefreshToken> AddRefreshToken(string refreshToken, User user);
        Task<TokensCortage> RefreshToken(string token, string refreshToken);
    }
}