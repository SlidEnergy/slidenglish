using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web
{
    public interface ITokenService
    {
        Task<RefreshToken> AddRefreshToken(string refreshToken, User user);
        Task<TokensCortage> RefreshToken(string token, string refreshToken);
		Task<TokensCortage> Login(string email, string password);
	}
}