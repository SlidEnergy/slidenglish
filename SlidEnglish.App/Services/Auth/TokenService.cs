using Microsoft.IdentityModel.Tokens;
using SlidEnglish.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
	public class TokenService : ITokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly AuthSettings _authSettings;
		private readonly IAuthTokenService _service;

		public TokenService(ITokenGenerator tokenGenerator, AuthSettings authSettings, IAuthTokenService service)
        {
            _tokenGenerator = tokenGenerator;
            _authSettings = authSettings;
			_service = service;
        }

        public async Task<TokensCortage> RefreshToken(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userId = principal.GetUserId();
			var savedToken = await _service.FindTokenAsync(refreshToken);

            if (savedToken == null || savedToken.User.Id != userId)
                throw new SecurityTokenException("Invalid refresh token");

            var newToken = _tokenGenerator.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

			await _service.UpdateToken(savedToken, newRefreshToken);

            return new TokensCortage() { Token = newToken, RefreshToken = newRefreshToken };
        }

        public async Task<RefreshToken> AddRefreshToken(string refreshToken, User user)
        {
			return await _service.AddToken(user, refreshToken);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authSettings.GetSymmetricSecurityKey(),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
