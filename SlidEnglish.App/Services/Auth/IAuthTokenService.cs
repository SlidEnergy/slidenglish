using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.App
{
	public interface IAuthTokenService
	{
		Task<RefreshToken> AddToken(User user, string token);
		Task<RefreshToken> FindTokenAsync(string token);
		Task<RefreshToken> UpdateToken(RefreshToken oldToken, string newToken);
	}
}