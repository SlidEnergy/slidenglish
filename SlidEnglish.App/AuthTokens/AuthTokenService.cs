using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
	public class AuthTokenService : IAuthTokenService
	{
		private readonly IApplicationDbContext _context;

		public AuthTokenService(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<RefreshToken> FindTokenAsync(string token)
		{
			return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
		}

		public async Task<RefreshToken> AddToken(User user, string token)
		{
			var existToken = await FindTokenAsync(token);

			if (existToken == null)
			{
				var newToken = new RefreshToken("any", token, user);
				_context.RefreshTokens.Add(newToken);
				await _context.SaveChangesAsync();
				return newToken;
			}

			return existToken;
		}

		public async Task<RefreshToken> UpdateToken(RefreshToken oldToken, string newToken)
		{
			oldToken.Update("any", newToken);
			_context.RefreshTokens.Update(oldToken);
			await _context.SaveChangesAsync();

			return oldToken;
		}
	}
}
